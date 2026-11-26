using MediatR;
using StorageStrategy.Domain.Commands.Print;
using StorageStrategy.Domain.Repository;
using StorageStrategy.Domain.Services.Printing;
using StorageStrategy.Models;

namespace StorageStrategy.Domain.Handlers.Print;

/// <summary>
/// Usado pelo instalador do agente para confirmar a chave e mostrar a empresa. Não mexe na fila.
/// CompanyId e PrintStationId vêm da chave da estação.
/// </summary>
public class PingPrintStationHandler : HandlerBase, IRequestHandler<PingPrintStationQuery, Result>
{
    private readonly IPrintStationRepository _stations;
    private readonly ICompanyRepository _companies;
    private readonly TimeProvider _time;

    public PingPrintStationHandler(IPrintStationRepository stations, ICompanyRepository companies, TimeProvider time)
    {
        _stations = stations;
        _companies = companies;
        _time = time;
    }

    public async Task<Result> Handle(PingPrintStationQuery request, CancellationToken cancellationToken)
    {
        var station = await _stations.GetActiveByIdAsync(request.PrintStationId);
        if (station is null || station.CompanyId != request.CompanyId)
            return CreateError("Estação não encontrada");

        var now = PrintClock.Now(_time);
        if (station.LastSeenAt is null || now - station.LastSeenAt.Value >= ClaimPrintJobHandler.HeartbeatThrottle)
        {
            station.LastSeenAt = now;
            _stations.Update(station);
            await _stations.SaveAsync();
        }

        var company = await _companies.GetById(station.CompanyId);

        return CreateResponse(
            new PrintStationPingResponse(station.PrintStationId, station.Name, station.CompanyId, company?.Name ?? string.Empty),
            "Estação conectada");
    }
}
