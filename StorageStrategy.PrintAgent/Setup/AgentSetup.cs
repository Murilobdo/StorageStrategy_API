using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using StorageStrategy.PrintAgent.Printing;
using StorageStrategy.Printer.EscPos;
using StorageStrategy.Printer;

namespace StorageStrategy.PrintAgent.Setup;

/// <summary>
/// Modos usados pelo instalador (setup.exe):
/// <list type="bullet">
/// <item><c>--setup</c>: login do gerente, cadastra a estação (ou gera chave nova), confirma a chave e grava o appsettings.json.</item>
/// <item><c>--verify</c>: confirma, com a chave gravada, que a estação está conectada.</item>
/// <item><c>--print-check</c>: imprime um papel curto de "instalação concluída".</item>
/// </list>
/// O resultado vai para um arquivo (<c>--result</c>) em linhas <c>chave=valor</c>, lido pelo instalador.
/// A senha chega por arquivo temporário (<c>--password-file</c>), apagado logo após a leitura.
/// </summary>
public static class AgentSetup
{
    public const int Ok = 0;
    public const int InvalidArgs = 2;
    public const int LoginFailed = 10;
    public const int NotManager = 11;
    public const int Network = 12;
    public const int ApiError = 13;
    public const int PrinterError = 14;

    private static readonly JsonSerializerOptions Json = new(JsonSerializerDefaults.Web);

    public static bool Handles(string[] args) =>
        args.Contains("--setup") || args.Contains("--verify") || args.Contains("--print-check");

    public static async Task<int> RunAsync(string[] args)
    {
        var a = ParseArgs(args);
        var result = new SetupResult(Get(a, "--result"));

        try
        {
            if (args.Contains("--setup"))
                return await SetupAsync(a, result);
            if (args.Contains("--verify"))
                return await VerifyAsync(a, result);
            return PrintCheck(a, result);
        }
        catch (HttpRequestException ex)
        {
            return result.Fail(Network, $"Sem conexão com o servidor do StorageStrategy. Verifique a internet. ({ex.Message})");
        }
        catch (TaskCanceledException)
        {
            return result.Fail(Network, "O servidor do StorageStrategy não respondeu a tempo. Verifique a internet e tente de novo.");
        }
        catch (Exception ex)
        {
            return result.Fail(ApiError, $"Erro inesperado: {ex.Message}");
        }
    }

    private static async Task<int> SetupAsync(Dictionary<string, string> a, SetupResult result)
    {
        var api = Get(a, "--api");
        var email = Get(a, "--email");
        var passwordFile = Get(a, "--password-file");
        var printer = Get(a, "--printer");
        var stationName = Get(a, "--station-name") ?? $"Caixa - {Environment.MachineName}";
        var config = Get(a, "--config");

        if (api is null || email is null || passwordFile is null || printer is null || config is null)
            return result.Fail(InvalidArgs, "Parâmetros incompletos para a configuração.");

        string password;
        try
        {
            password = File.ReadAllText(passwordFile, Encoding.UTF8).TrimEnd('\r', '\n').TrimStart('﻿');
        }
        finally
        {
            TryDelete(passwordFile);
        }

        using var http = CreateClient(api);

        // 1. Login do gerente
        using var login = await http.PostAsJsonAsync("Account/login", new { email, password }, Json);
        var loginBody = await ReadJson(login);
        if (loginBody?["success"]?.GetValue<bool>() != true)
            return result.Fail(LoginFailed, Message(loginBody, "E-mail ou senha incorretos."));

        var token = loginBody["response"]?["token"]?.GetValue<string>();
        var role = loginBody["response"]?["employee"]?["jobRole"]?.GetValue<int>();
        if (string.IsNullOrEmpty(token))
            return result.Fail(LoginFailed, "O servidor não devolveu o acesso. Tente de novo.");
        if (role is not (1 or 7)) // Manager ou Admin
            return result.Fail(NotManager, "Use o login de um GERENTE da empresa para instalar a impressora.");

        http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        // 2. Cadastra/atualiza a estação e obtém a chave
        var current = await ReadJson(await http.GetAsync("print/station"));
        var alreadyConfigured = current?["response"]?["isConfigured"]?.GetValue<bool>() == true;
        var encoding = current?["response"]?["encoding"]?.GetValue<int>() ?? 1;

        using var save = await http.PutAsJsonAsync("print/station", new { name = stationName, paperWidthMm = 58, encoding }, Json);
        var saveBody = await ReadJson(save);
        if (saveBody?["success"]?.GetValue<bool>() != true)
            return result.Fail(ApiError, Message(saveBody, "Não foi possível cadastrar a impressora no StorageStrategy."));

        var key = saveBody["response"]?["stationKey"]?.GetValue<string>();
        if (alreadyConfigured || string.IsNullOrEmpty(key))
        {
            using var regen = await http.PostAsync("print/station/regenerate-key", JsonContent.Create(new { }));
            var regenBody = await ReadJson(regen);
            key = regenBody?["response"]?["stationKey"]?.GetValue<string>();
            if (string.IsNullOrEmpty(key))
                return result.Fail(ApiError, Message(regenBody, "Não foi possível gerar a chave da impressora."));
        }

        // 3. Confirma a chave nova (e descobre o nome da empresa)
        var ping = await PingAsync(api, key);
        if (ping is null)
            return result.Fail(ApiError, "A chave gerada não foi aceita pelo servidor. Tente de novo.");

        // 4. Grava a configuração do agente
        WriteConfig(config, api, key, printer);

        result.Set("company", ping.Value.Company);
        result.Set("station", ping.Value.Station);
        result.Set("replaced", alreadyConfigured ? "1" : "0");
        return result.Success("Impressora cadastrada no StorageStrategy.");
    }

    private static async Task<int> VerifyAsync(Dictionary<string, string> a, SetupResult result)
    {
        var configPath = Get(a, "--config") ?? Path.Combine(AppContext.BaseDirectory, "appsettings.json");
        var node = JsonNode.Parse(File.ReadAllText(configPath))?["PrintAgent"];
        var api = node?["ApiBaseUrl"]?.GetValue<string>();
        var key = node?["StationKey"]?.GetValue<string>();
        if (string.IsNullOrEmpty(api) || string.IsNullOrEmpty(key))
            return result.Fail(InvalidArgs, "Configuração do agente incompleta.");

        var ping = await PingAsync(api, key);
        if (ping is null)
            return result.Fail(ApiError, "A estação não foi aceita pelo servidor (chave inválida).");

        result.Set("company", ping.Value.Company);
        result.Set("station", ping.Value.Station);
        return result.Success("Estação conectada.");
    }

    private static int PrintCheck(Dictionary<string, string> a, SetupResult result)
    {
        var printerName = Get(a, "--printer");
        if (printerName is null)
            return result.Fail(InvalidArgs, "Informe a impressora.");

        var printer = new RawPrinter();
        var problem = printer.GetProblem(printerName);
        if (problem is not null)
            return result.Fail(PrinterError, problem);

        var company = Get(a, "--company") ?? string.Empty;
        var bytes = new EscPosBuilder(ReceiptEncoding.Cp860)
            .Initialize()
            .SelectCodePage()
            .Bold(true).Line(TextLayout.Center("INSTALAÇÃO CONCLUÍDA", 32)).Bold(false)
            .Line(new string('-', 32))
            .Line("Agente de impressão")
            .Line("StorageStrategy")
            .Line(company)
            .Line(DateTime.Now.ToString("dd/MM/yyyy HH:mm"))
            .Line(new string('-', 32))
            .Line("Esta impressora está pronta para")
            .Line("imprimir as comandas.")
            .Feed(4)
            .ToArray();

        printer.Print(printerName, bytes, "StorageStrategy - teste de instalação");
        return result.Success("Papel de teste enviado.");
    }

    private static async Task<(string Company, string Station)?> PingAsync(string api, string key)
    {
        using var http = CreateClient(api);
        http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("PrintStation", key);
        using var response = await http.GetAsync("print/agent/ping");
        if (!response.IsSuccessStatusCode)
            return null;

        var body = await ReadJson(response);
        return (body?["companyName"]?.GetValue<string>() ?? string.Empty,
                body?["stationName"]?.GetValue<string>() ?? string.Empty);
    }

    private static void WriteConfig(string path, string api, string key, string printer)
    {
        var config = new JsonObject
        {
            ["Logging"] = new JsonObject
            {
                ["LogLevel"] = new JsonObject
                {
                    ["Default"] = "Information",
                    ["Microsoft.Hosting.Lifetime"] = "Information",
                    ["System.Net.Http.HttpClient"] = "Warning"
                }
            },
            ["PrintAgent"] = new JsonObject
            {
                ["ApiBaseUrl"] = api,
                ["StationKey"] = key,
                ["PrinterName"] = printer,
                ["PollIntervalSeconds"] = 2
            }
        };

        Directory.CreateDirectory(Path.GetDirectoryName(Path.GetFullPath(path))!);
        File.WriteAllText(path, config.ToJsonString(new JsonSerializerOptions { WriteIndented = true }), new UTF8Encoding(false));
    }

    private static HttpClient CreateClient(string api) => new()
    {
        BaseAddress = new Uri(api.EndsWith('/') ? api : api + "/"),
        Timeout = TimeSpan.FromSeconds(30)
    };

    private static async Task<JsonNode?> ReadJson(HttpResponseMessage response)
    {
        var text = await response.Content.ReadAsStringAsync();
        if (string.IsNullOrWhiteSpace(text))
            return null;
        try { return JsonNode.Parse(text); }
        catch (JsonException) { return null; }
    }

    private static string Message(JsonNode? body, string fallback)
    {
        var error = body?["errorMessage"]?.GetValue<string>();
        if (string.IsNullOrWhiteSpace(error))
            error = body?["errors"]?.AsArray().FirstOrDefault()?["errorMessage"]?.GetValue<string>();
        return string.IsNullOrWhiteSpace(error) ? fallback : error;
    }

    private static Dictionary<string, string> ParseArgs(string[] args)
    {
        var map = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        for (var i = 0; i < args.Length - 1; i++)
        {
            if (args[i].StartsWith("--") && !args[i + 1].StartsWith("--"))
                map[args[i]] = args[++i];
        }
        return map;
    }

    private static string? Get(Dictionary<string, string> a, string key) =>
        a.TryGetValue(key, out var v) && !string.IsNullOrWhiteSpace(v) ? v.Trim() : null;

    private static void TryDelete(string path)
    {
        try { File.Delete(path); } catch { /* o instalador apaga a pasta temporária de qualquer forma */ }
    }

    /// <summary>Arquivo chave=valor em UTF-8 com BOM, lido pelo script do Inno Setup.</summary>
    private sealed class SetupResult
    {
        private readonly string? _path;
        private readonly Dictionary<string, string> _values = new();

        public SetupResult(string? path) => _path = path;

        public void Set(string key, string value) => _values[key] = value.Replace('\r', ' ').Replace('\n', ' ');

        public int Success(string message) => Write("ok", message, Ok);

        public int Fail(int code, string message) => Write("error", message, code);

        private int Write(string status, string message, int code)
        {
            _values["status"] = status;
            _values["message"] = message.Replace('\r', ' ').Replace('\n', ' ');
            Console.WriteLine(message);
            if (_path is not null)
                File.WriteAllLines(_path, _values.Select(kv => $"{kv.Key}={kv.Value}"), new UTF8Encoding(true));
            return code;
        }
    }
}
