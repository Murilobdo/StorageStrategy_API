using MediatR;
using StorageStrategy.Domain.Commands.PixCredential;
using StorageStrategy.Domain.Repository;
using StorageStrategy.Models;
using StorageStrategy.Utils.Services;

namespace StorageStrategy.Domain.Handlers.PixCredential;

public class SavePixCredentialHandler : HandlerBase, IRequestHandler<SavePixCredentialCommand, Result>
{
    private readonly IPixCredentialRepository _repo;
    private readonly ISecretProtector _protector;

    public SavePixCredentialHandler(IPixCredentialRepository repo, ISecretProtector protector)
    {
        _repo = repo;
        _protector = protector;
    }

    public async Task<Result> Handle(SavePixCredentialCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid())
            return CreateError(request.GetErros(), "Dados inválidos");

        var entity = await _repo.GetByCompanyIdAsync(request.CompanyId);
        var isNew = entity is null;
        entity ??= new PixCredentialEntity { CompanyId = request.CompanyId };

        entity.AccessTokenEncrypted = _protector.Protect(request.AccessToken);
        entity.AccessTokenLastDigits = request.AccessToken.Length >= 4
            ? request.AccessToken[^4..]
            : request.AccessToken;
        entity.WebhookSecretEncrypted = _protector.Protect(request.WebhookSecret);
        entity.IsActive = true;
        entity.AtualizadoEm = DateTime.Now;
        entity.AtualizadoPorEmployeeId = request.EmployeeId;

        if (isNew)
            await _repo.AddAsync(entity);
        else
            _repo.Update(entity);

        await _repo.SaveAsync();

        return CreateResponse(ToStatusResponse(entity), "Credenciais do Mercado Pago salvas com sucesso!");
    }

    public static PixCredentialStatusResponse ToStatusResponse(PixCredentialEntity entity)
    {
        return new PixCredentialStatusResponse
        {
            IsConfigured = true,
            IsActive = entity.IsActive,
            AccessTokenLastDigits = entity.AccessTokenLastDigits,
            AtualizadoEm = entity.AtualizadoEm
        };
    }
}
