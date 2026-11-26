using MediatR;
using StorageStrategy.Domain.Commands.Print;
using StorageStrategy.Domain.Repository;
using StorageStrategy.Domain.Services.Printing;
using StorageStrategy.Models;

namespace StorageStrategy.Domain.Handlers.Print;

public class GetLatestPrintJobHandler : HandlerBase, IRequestHandler<GetLatestPrintJobQuery, Result>
{
    private readonly IPrintJobRepository _jobs;
    private readonly IPrintStationRepository _stations;
    private readonly TimeProvider _time;

    public GetLatestPrintJobHandler(IPrintJobRepository jobs, IPrintStationRepository stations, TimeProvider time)
    {
        _jobs = jobs;
        _stations = stations;
        _time = time;
    }

    public async Task<Result> Handle(GetLatestPrintJobQuery request, CancellationToken cancellationToken)
    {
        var job = await _jobs.GetLatestByCommandAsync(request.CommandId, request.CompanyId);
        if (job is null)
            return new Result((object)null!, "Nenhuma impressão");

        var station = await _stations.GetActiveByCompanyIdAsync(request.CompanyId);
        var online = station?.IsOnline(PrintClock.Now(_time)) ?? false;

        return CreateResponse(PrintJobResponse.From(job, online), "Busca realizada");
    }
}
