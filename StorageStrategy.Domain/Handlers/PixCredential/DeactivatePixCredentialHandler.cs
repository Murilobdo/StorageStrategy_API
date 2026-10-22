using MediatR;
using StorageStrategy.Domain.Commands.PixCredential;
using StorageStrategy.Domain.Repository;
using StorageStrategy.Models;

namespace StorageStrategy.Domain.Handlers.PixCredential;

public class DeactivatePixCredentialHandler : HandlerBase, IRequestHandler<DeactivatePixCredentialCommand, Result>
{
    private readonly IPixCredentialRepository _repo;

    public DeactivatePixCredentialHandler(IPixCredentialRepository repo)
    {
        _repo = repo;
    }

    public async Task<Result> Handle(DeactivatePixCredentialCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repo.GetByCompanyIdAsync(request.CompanyId);
        if (entity is null)
            return CreateError("Nenhuma credencial encontrada para esta empresa");

        entity.IsActive = false;
        entity.AtualizadoEm = DateTime.Now;
        _repo.Update(entity);
        await _repo.SaveAsync();

        return CreateResponse(SavePixCredentialHandler.ToStatusResponse(entity), "Credenciais do Mercado Pago desativadas");
    }
}
