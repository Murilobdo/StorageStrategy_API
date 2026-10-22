using MediatR;
using StorageStrategy.Domain.Commands.PixCharge;
using StorageStrategy.Domain.Repository;
using StorageStrategy.Models;

namespace StorageStrategy.Domain.Handlers.PixCharge;

public class GetPixChargeStatusHandler : HandlerBase, IRequestHandler<GetPixChargeStatusQuery, Result>
{
    private readonly IPixChargeRepository _repo;

    public GetPixChargeStatusHandler(IPixChargeRepository repo)
    {
        _repo = repo;
    }

    public async Task<Result> Handle(GetPixChargeStatusQuery request, CancellationToken cancellationToken)
    {
        var pixCharge = await _repo.GetByIdForCompanyAsync(request.PixChargeId, request.CompanyId);
        if (pixCharge is null)
            return CreateError("Cobrança Pix não encontrada");

        return CreateResponse(pixCharge, "Busca realizada");
    }
}
