using Microsoft.Extensions.Options;
using StorageStrategy.PrintAgent;
using StorageStrategy.PrintAgent.Api;
using StorageStrategy.PrintAgent.Printing;
using StorageStrategy.PrintAgent.Setup;

// Agente de impressão (feature 006): roda no computador do caixa, ao lado da impressora USB.
// Serviço do Windows: sc.exe create StorageStrategyPrintAgent binPath= "C:\...\StorageStrategy.PrintAgent.exe"
// Console:            StorageStrategy.PrintAgent.exe
// Página de teste:    StorageStrategy.PrintAgent.exe --test-page

Console.OutputEncoding = System.Text.Encoding.UTF8;

// Modos do instalador: --setup, --verify, --print-check (não sobem o serviço).
if (AgentSetup.Handles(args))
    return await AgentSetup.RunAsync(args);

var builder = Host.CreateApplicationBuilder(new HostApplicationBuilderSettings
{
    Args = args,
    ContentRootPath = AppContext.BaseDirectory
});

builder.Services.AddWindowsService(o => o.ServiceName = "StorageStrategyPrintAgent");
builder.Services.Configure<PrintAgentOptions>(builder.Configuration.GetSection(PrintAgentOptions.Section));
builder.Services.AddSingleton<IRawPrinter, RawPrinter>();
builder.Services.AddHttpClient<PrintApiClient>();
builder.Services.AddHostedService<PrintAgentWorker>();

if (OperatingSystem.IsWindows())
    builder.Logging.AddEventLog(o => o.SourceName = "StorageStrategyPrintAgent");

var options = builder.Configuration.GetSection(PrintAgentOptions.Section).Get<PrintAgentOptions>() ?? new PrintAgentOptions();

if (args.Contains("--test-page"))
    return TestPage.Run(options.PrinterName, new RawPrinter());

var errors = options.Validate().ToList();
if (errors.Count > 0)
{
    foreach (var error in errors)
        Console.Error.WriteLine(error);
    return 1;
}

await builder.Build().RunAsync();
return 0;
