using MediatR;
using StorageStrategy.Domain.Commands.PixCredential;
using StorageStrategy.Domain.Repository;
using StorageStrategy.Models;

namespace StorageStrategy.Domain.Handlers.PixCredential;

public class GetPixCredentialStatusHandler : HandlerBase, IRequestHandler<GetPixCredentialStatusQuery, Result>
{
    private readonly IPixCredentialRepository _repo;

    public GetPixCredentialStatusHandler(IPixCredentialRepository repo)
    {
        _repo = repo;
    }

    public async Task<Result> Handle(GetPixCredentialStatusQuery request, CancellationToken cancellationToken)
    {
        var entity = await _repo.GetByCompanyIdAsync(request.CompanyId);

        if (entity is null)
            return CreateResponse(new PixCredentialStatusResponse { IsConfigured = false, IsActive = false }, "Busca realizada");

        return CreateResponse(SavePixCredentialHandler.ToStatusResponse(entity), "Busca realizada");
    }
}
