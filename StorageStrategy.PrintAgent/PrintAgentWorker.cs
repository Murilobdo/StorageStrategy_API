using Microsoft.Extensions.Options;
using StorageStrategy.PrintAgent.Api;
using StorageStrategy.PrintAgent.Printing;

namespace StorageStrategy.PrintAgent;

/// <summary>Ciclo do agente (contracts/print-agent.md): busca → imprime → informa resultado.</summary>
public class PrintAgentWorker : BackgroundService
{
    private static readonly TimeSpan UnauthorizedWait = TimeSpan.FromSeconds(60);
    private static readonly TimeSpan MaxBackoff = TimeSpan.FromSeconds(60);

    private readonly PrintApiClient _api;
    private readonly IRawPrinter _printer;
    private readonly PrintAgentOptions _options;
    private readonly ILogger<PrintAgentWorker> _logger;

    public PrintAgentWorker(PrintApiClient api, IRawPrinter printer, IOptions<PrintAgentOptions> options, ILogger<PrintAgentWorker> logger)
    {
        _api = api;
        _printer = printer;
        _options = options.Value;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var poll = TimeSpan.FromSeconds(_options.PollIntervalSeconds);
        var backoff = TimeSpan.FromSeconds(2);

        _logger.LogInformation("Agente de impressão iniciado: impressora \"{PrinterName}\", chave {StationKey}",
            _options.PrinterName, _options.StationKeyHint);

        while (!stoppingToken.IsCancellationRequested)
        {
            TimeSpan wait;
            try
            {
                var claim = await _api.ClaimAsync(stoppingToken);
                switch (claim.Status)
                {
                    case ClaimStatus.Job:
                        await PrintAsync(claim.Job!, stoppingToken);
                        backoff = TimeSpan.FromSeconds(2);
                        continue; // esvazia a fila antes de esperar

                    case ClaimStatus.Empty:
                        backoff = TimeSpan.FromSeconds(2);
                        wait = poll;
                        break;

                    case ClaimStatus.Unauthorized:
                        _logger.LogError("Chave da estação inválida. Gere uma nova na tela de configuração da impressora e atualize o appsettings.json.");
                        wait = UnauthorizedWait;
                        break;

                    case ClaimStatus.RateLimited:
                        wait = claim.RetryAfter ?? backoff;
                        break;

                    default:
                        wait = backoff;
                        backoff = TimeSpan.FromSeconds(Math.Min(backoff.TotalSeconds * 2, MaxBackoff.TotalSeconds));
                        break;
                }
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro inesperado no ciclo de impressão");
                wait = backoff;
            }

            try
            {
                await Task.Delay(wait, stoppingToken);
            }
            catch (OperationCanceledException)
            {
                break;
            }
        }

        _logger.LogInformation("Agente de impressão parado");
    }

    private async Task PrintAsync(ClaimedJob job, CancellationToken ct)
    {
        _logger.LogInformation("Imprimindo job {PrintJobId} (via {Copy})", job.PrintJobId, job.Copy);

        string? failure = _printer.GetProblem(_options.PrinterName);
        if (failure is null)
        {
            try
            {
                _printer.Print(_options.PrinterName, Convert.FromBase64String(job.PayloadBase64), $"Comanda - job {job.PrintJobId}");
            }
            catch (Exception ex)
            {
                failure = ex.Message;
                _logger.LogError(ex, "Falha ao imprimir o job {PrintJobId}", job.PrintJobId);
            }
        }
        else
        {
            _logger.LogWarning("Job {PrintJobId} não impresso: {Failure}", job.PrintJobId, failure);
        }

        var success = failure is null;
        if (await _api.ReportAsync(job, success, failure, ct))
            _logger.LogInformation("Job {PrintJobId}: {Result}", job.PrintJobId, success ? "Impresso" : "Falhou");
    }
}
