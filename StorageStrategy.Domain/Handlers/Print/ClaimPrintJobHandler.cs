using MediatR;
using Microsoft.Extensions.Logging;
using StorageStrategy.Domain.Commands.Print;
using StorageStrategy.Domain.Repository;
using StorageStrategy.Domain.Services.Printing;
using StorageStrategy.Models;

namespace StorageStrategy.Domain.Handlers.Print;

/// <summary>Agente pega o próximo job da empresa da estação. Response nulo = fila vazia (204).</summary>
public class ClaimPrintJobHandler : HandlerBase, IRequestHandler<ClaimPrintJobCommand, Result>
{
    /// <summary>Intervalo mínimo entre gravações do "último contato" (evita escrita a cada poll).</summary>
    public static readonly TimeSpan HeartbeatThrottle = TimeSpan.FromSeconds(15);

    private readonly IPrintJobRepository _jobs;
    private readonly IPrintStationRepository _stations;
    private readonly TimeProvider _time;
    private readonly ILogger<ClaimPrintJobHandler> _logger;

    public ClaimPrintJobHandler(
        IPrintJobRepository jobs,
        IPrintStationRepository stations,
        TimeProvider time,
        ILogger<ClaimPrintJobHandler> logger)
    {
        _jobs = jobs;
        _stations = stations;
        _time = time;
        _logger = logger;
    }

    public async Task<Result> Handle(ClaimPrintJobCommand request, CancellationToken cancellationToken)
    {
        var station = await _stations.GetActiveByIdAsync(request.PrintStationId);
        if (station is null || station.CompanyId != request.CompanyId)
            return CreateError("Estação não encontrada");

        var now = PrintClock.Now(_time);
        if (station.LastSeenAt is null || now - station.LastSeenAt.Value >= HeartbeatThrottle)
        {
            station.LastSeenAt = now;
            _stations.Update(station);
            await _stations.SaveAsync();
        }

        var claimId = Guid.NewGuid();
        var job = await _jobs.ClaimNextAsync(station.CompanyId, station.PrintStationId, claimId, now);
        if (job is null)
            return new Result((object)null!, "Fila vazia");

        _logger.LogInformation(
            "Impressão reservada: empresa {CompanyId}, job {PrintJobId}, comanda {CommandId}, estação {PrintStationId}, status {OldStatus} -> {NewStatus}",
            job.CompanyId, job.PrintJobId, job.CommandId, station.PrintStationId, PrintJobStatusEnum.Aguardando, job.Status);

        return CreateResponse(
            new ClaimedPrintJobResponse(job.PrintJobId, claimId, Convert.ToBase64String(job.Payload), job.Copy),
            "Job reservado");
    }
}
