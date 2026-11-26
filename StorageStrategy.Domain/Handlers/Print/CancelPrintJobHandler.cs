using MediatR;
using Microsoft.Extensions.Logging;
using StorageStrategy.Domain.Commands.Print;
using StorageStrategy.Domain.Repository;
using StorageStrategy.Domain.Services.Printing;
using StorageStrategy.Models;

namespace StorageStrategy.Domain.Handlers.Print;

public class CancelPrintJobHandler : HandlerBase, IRequestHandler<CancelPrintJobCommand, Result>
{
    private readonly IPrintJobRepository _jobs;
    private readonly TimeProvider _time;
    private readonly ILogger<CancelPrintJobHandler> _logger;

    public CancelPrintJobHandler(IPrintJobRepository jobs, TimeProvider time, ILogger<CancelPrintJobHandler> logger)
    {
        _jobs = jobs;
        _time = time;
        _logger = logger;
    }

    public async Task<Result> Handle(CancelPrintJobCommand request, CancellationToken cancellationToken)
    {
        var job = await _jobs.GetByIdForCompanyAsync(request.PrintJobId, request.CompanyId);
        if (job is null)
            return CreateError("Impressão não encontrada");

        if (job.Status != PrintJobStatusEnum.Aguardando)
            return CreateError("Só é possível cancelar pedidos aguardando");

        job.Status = PrintJobStatusEnum.Cancelado;
        job.CompletedAt = PrintClock.Now(_time);
        _jobs.Update(job);
        await _jobs.SaveAsync();

        _logger.LogInformation(
            "Impressão cancelada: empresa {CompanyId}, job {PrintJobId}, comanda {CommandId}, status {OldStatus} -> {NewStatus}",
            job.CompanyId, job.PrintJobId, job.CommandId, PrintJobStatusEnum.Aguardando, job.Status);

        return CreateResponse(PrintJobResponse.From(job, stationOnline: false), "Impressão cancelada");
    }
}
