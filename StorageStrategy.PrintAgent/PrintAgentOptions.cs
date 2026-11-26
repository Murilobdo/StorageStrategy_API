namespace StorageStrategy.PrintAgent;

public class PrintAgentOptions
{
    public const string Section = "PrintAgent";

    /// <summary>Ex.: https://api.seudominio.com.br/api/ (com a barra final).</summary>
    public string ApiBaseUrl { get; set; } = string.Empty;

    /// <summary>Chave da estação (ps_{id}_{segredo}). Nunca escrever em log.</summary>
    public string StationKey { get; set; } = string.Empty;

    /// <summary>Nome exato da impressora no Windows (ex.: POS-58).</summary>
    public string PrinterName { get; set; } = string.Empty;

    public int PollIntervalSeconds { get; set; } = 2;

    public string StationKeyHint =>
        StationKey.Length >= 4 ? $"...{StationKey[^4..]}" : "(não configurada)";

    public IEnumerable<string> Validate()
    {
        if (!Uri.TryCreate(ApiBaseUrl, UriKind.Absolute, out _))
            yield return "PrintAgent:ApiBaseUrl inválida";
        if (!StationKey.StartsWith("ps_", StringComparison.Ordinal))
            yield return "PrintAgent:StationKey ausente ou inválida (copie a chave exibida na tela de configuração da impressora)";
        if (string.IsNullOrWhiteSpace(PrinterName))
            yield return "PrintAgent:PrinterName ausente (nome da impressora no Windows)";
        if (PollIntervalSeconds < 1)
            yield return "PrintAgent:PollIntervalSeconds deve ser ao menos 1";
    }
}
