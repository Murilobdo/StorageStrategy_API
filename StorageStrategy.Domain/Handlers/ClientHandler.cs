using MediatR;
using StorageStrategy.Domain.Repository;
using StorageStrategy.Models;

namespace StorageStrategy.Domain.Handlers;

public class ClientHandler : HandlerBase,
    IRequestHandler<CreateClientCommand, Result>
{
    private IClientRepository _repo;
    public ClientHandler(IClientRepository repo)
    {
        _repo = repo;
    }
    
    public async Task<Result> Handle(CreateClientCommand request, CancellationToken cancellationToken)
    {
        request.Name = request.Name.ToUpper();
        
        var sameNameClient = await _repo.GetByNameAsync(request.CompanyId, request.Name);
        if (sameNameClient is not null)
            return CreateError("Cliente ja cadastrado com esse nome");

        var client = new ClientEntity(request.CompanyId, request.Name);
        await _repo.AddAsync(client);

        return CreateResponse(client, "Cliente cadastrado");
    }
}

