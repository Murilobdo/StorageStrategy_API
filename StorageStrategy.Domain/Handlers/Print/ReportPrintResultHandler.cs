using MediatR;
using Microsoft.Extensions.Logging;
using StorageStrategy.Domain.Commands.Print;
using StorageStrategy.Domain.Repository;
using StorageStrategy.Domain.Services.Printing;
using StorageStrategy.Models;

namespace StorageStrategy.Domain.Handlers.Print;

/// <summary>
/// Agente informa o resultado. Só aceito com o ClaimId da reserva atual; repetir o mesmo resultado é idempotente.
/// Response = <see cref="PrintResultOutcome"/> (o controller converte em 200/404/409).
/// </summary>
public class ReportPrintResultHandler : HandlerBase, IRequestHandler<ReportPrintResultCommand, Result>
{
    private readonly IPrintJobRepository _jobs;
    private readonly TimeProvider _time;
    private readonly ILogger<ReportPrintResultHandler> _logger;

    public ReportPrintResultHandler(IPrintJobRepository jobs, TimeProvider time, ILogger<ReportPrintResultHandler> logger)
    {
        _jobs = jobs;
        _time = time;
        _logger = logger;
    }

    public async Task<Result> Handle(ReportPrintResultCommand request, CancellationToken cancellationToken)
    {
        var job = await _jobs.GetByIdForCompanyAsync(request.PrintJobId, request.CompanyId);
        if (job is null)
            return CreateResponse(PrintResultOutcome.NotFound, "Impressão não encontrada");

        if (job.ClaimId != request.ClaimId)
            return CreateResponse(PrintResultOutcome.Conflict, "Reserva expirada: o job foi reentregue");

        if (job.IsFinal)
            return CreateResponse(PrintResultOutcome.Accepted, "Resultado já registrado");

        if (job.Status != PrintJobStatusEnum.Imprimindo)
            return CreateResponse(PrintResultOutcome.Conflict, "Job não está em impressão");

        var oldStatus = job.Status;
        job.Status = request.Success ? PrintJobStatusEnum.Impresso : PrintJobStatusEnum.Falhou;
        job.FailureReason = request.Success ? null : Truncate(request.FailureReason);
        job.CompletedAt = PrintClock.Now(_time);
        _jobs.Update(job);
        await _jobs.SaveAsync();

        _logger.LogInformation(
            "Resultado da impressão: empresa {CompanyId}, job {PrintJobId}, comanda {CommandId}, estação {PrintStationId}, status {OldStatus} -> {NewStatus}",
            job.CompanyId, job.PrintJobId, job.CommandId, job.PrintStationId, oldStatus, job.Status);

        return CreateResponse(PrintResultOutcome.Accepted, "Resultado registrado");
    }

    private static string Truncate(string? reason)
    {
        var text = string.IsNullOrWhiteSpace(reason) ? "Falha não informada pela impressora" : reason.Trim();
        return text.Length <= PrintJobEntity.FailureReasonMaxLength
            ? text
            : text[..PrintJobEntity.FailureReasonMaxLength];
    }
}
