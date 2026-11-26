using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using StorageStrategy.Domain.Repository;
using StorageStrategy.Models;

namespace StorageStrategy.Domain.Services.Printing;

/// <summary>Regras de manutenção da fila (research R8), separadas do BackgroundService para teste.</summary>
public class PrintJobMaintenance
{
    public static readonly TimeSpan WaitingExpiration = TimeSpan.FromMinutes(30);
    public static readonly TimeSpan ClaimTimeout = TimeSpan.FromMinutes(2);
    public static readonly TimeSpan Retention = TimeSpan.FromDays(30);

    private readonly IPrintJobRepository _jobs;
    private readonly TimeProvider _time;
    private readonly ILogger<PrintJobMaintenance> _logger;

    public PrintJobMaintenance(IPrintJobRepository jobs, TimeProvider time, ILogger<PrintJobMaintenance> logger)
    {
        _jobs = jobs;
        _time = time;
        _logger = logger;
    }

    public async Task RunAsync(bool purgeOld)
    {
        var now = PrintClock.Now(_time);

        // Entidades carregadas com tracking: SaveAsync grava todas (sem Update, que limpa o ChangeTracker).
        var expired = await _jobs.GetWaitingCreatedBeforeAsync(now - WaitingExpiration);
        foreach (var job in expired)
        {
            job.Status = PrintJobStatusEnum.Expirado;
            job.CompletedAt = now;
            _logger.LogInformation(
                "Impressão expirada: empresa {CompanyId}, job {PrintJobId}, comanda {CommandId}, status {OldStatus} -> {NewStatus}",
                job.CompanyId, job.PrintJobId, job.CommandId, PrintJobStatusEnum.Aguardando, job.Status);
        }

        var stale = await _jobs.GetClaimedBeforeAsync(now - ClaimTimeout);
        foreach (var job in stale)
        {
            _logger.LogWarning(
                "Impressão sem resultado do agente, voltando à fila: empresa {CompanyId}, job {PrintJobId}, comanda {CommandId}, estação {PrintStationId}, status {OldStatus} -> {NewStatus}",
                job.CompanyId, job.PrintJobId, job.CommandId, job.PrintStationId, job.Status, PrintJobStatusEnum.Aguardando);
            job.Status = PrintJobStatusEnum.Aguardando;
            job.ClaimId = null;
            job.ClaimedAt = null;
            job.PrintStationId = null;
        }

        if (expired.Count > 0 || stale.Count > 0)
            await _jobs.SaveAsync();

        if (purgeOld)
        {
            var removed = await _jobs.DeleteCreatedBeforeAsync(now - Retention);
            if (removed > 0)
                _logger.LogInformation("Histórico de impressão limpo: {Removed} jobs com mais de 30 dias", removed);
        }
    }
}

public class PrintJobMaintenanceJob : BackgroundService
{
    private static readonly TimeSpan Interval = TimeSpan.FromMinutes(1);
    private static readonly TimeSpan PurgeInterval = TimeSpan.FromDays(1);

    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<PrintJobMaintenanceJob> _logger;
    private DateTime _lastPurge = DateTime.MinValue;

    public PrintJobMaintenanceJob(IServiceScopeFactory scopeFactory, ILogger<PrintJobMaintenanceJob> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var timer = new PeriodicTimer(Interval);

        do
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();
                var maintenance = scope.ServiceProvider.GetRequiredService<PrintJobMaintenance>();

                var purge = DateTime.UtcNow - _lastPurge >= PurgeInterval;
                await maintenance.RunAsync(purge);
                if (purge)
                    _lastPurge = DateTime.UtcNow;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Falha na manutenção da fila de impressão");
            }
        }
        while (!stoppingToken.IsCancellationRequested && await timer.WaitForNextTickAsync(stoppingToken));
    }
}
