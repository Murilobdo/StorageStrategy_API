using MediatR;
using Microsoft.Extensions.Logging;
using StorageStrategy.Domain.Commands.Print;
using StorageStrategy.Domain.Repository;
using StorageStrategy.Domain.Services.Printing;
using StorageStrategy.Models;
using StorageStrategy.Printer;

namespace StorageStrategy.Domain.Handlers.Print;

public class CreatePrintJobHandler : HandlerBase, IRequestHandler<CreatePrintJobCommand, Result>
{
    /// <summary>Toques repetidos do mesmo funcionário na mesma comanda dentro desta janela viram um só job (FR-010).</summary>
    public static readonly TimeSpan DuplicateWindow = TimeSpan.FromSeconds(3);

    private readonly IPrintJobRepository _jobs;
    private readonly IPrintStationRepository _stations;
    private readonly IReceiptRenderer _renderer;
    private readonly TimeProvider _time;
    private readonly ILogger<CreatePrintJobHandler> _logger;

    public CreatePrintJobHandler(
        IPrintJobRepository jobs,
        IPrintStationRepository stations,
        IReceiptRenderer renderer,
        TimeProvider time,
        ILogger<CreatePrintJobHandler> logger)
    {
        _jobs = jobs;
        _stations = stations;
        _renderer = renderer;
        _time = time;
        _logger = logger;
    }

    public async Task<Result> Handle(CreatePrintJobCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid())
            return CreateError(request.GetErros(), "Dados inválidos");

        // Comanda de outra empresa responde igual a inexistente (não revela existência).
        var command = await _jobs.GetCommandForPrintAsync(request.CommandId, request.CompanyId);
        if (command is null)
            return CreateError("Comanda não encontrada");

        if (command.Items is null || command.Items.Count == 0)
            return CreateError("Adicione itens para imprimir");

        var station = await _stations.GetActiveByCompanyIdAsync(request.CompanyId);
        if (station is null)
            return CreateError("Nenhuma impressora configurada");

        var now = PrintClock.Now(_time);
        var stationOnline = station.IsOnline(now);

        var duplicate = await _jobs.GetRecentDuplicateAsync(
            request.CommandId, request.EmployeeId, request.CompanyId, now - DuplicateWindow);
        if (duplicate is not null)
            return CreateResponse(PrintJobResponse.From(duplicate, stationOnline), "Enviado para impressão");

        var copy = await _jobs.CountPrintedAsync(request.CommandId, request.CompanyId) + 1;
        var document = ReceiptDocumentFactory.Create(command, copy);

        var job = new PrintJobEntity
        {
            CompanyId = request.CompanyId,
            CommandId = request.CommandId,
            RequestedByEmployeeId = request.EmployeeId,
            Copy = copy,
            Payload = _renderer.RenderEscPos(document, ReceiptDocumentFactory.Options(station)),
            Status = PrintJobStatusEnum.Aguardando,
            CreatedAt = now
        };

        await _jobs.AddAsync(job);
        await _jobs.SaveAsync();

        _logger.LogInformation(
            "Impressão solicitada: empresa {CompanyId}, job {PrintJobId}, comanda {CommandId}, via {Copy}, estação {PrintStationId} online {StationOnline}, status - -> {NewStatus}",
            job.CompanyId, job.PrintJobId, job.CommandId, job.Copy, station.PrintStationId, stationOnline, job.Status);

        return CreateResponse(PrintJobResponse.From(job, stationOnline), "Enviado para impressão");
    }
}
