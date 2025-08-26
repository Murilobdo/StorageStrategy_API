using MediatR;
using StorageStrategy.Domain.Commands;
using StorageStrategy.Domain.Commands.Client;
using StorageStrategy.Domain.Repository;
using StorageStrategy.Models;
using StorageStrategy.Models.ViewModels.Client;

namespace StorageStrategy.Domain.Handlers;

public class ClientHandler : HandlerBase,
    IRequestHandler<CreateClientCommand, Result>,
    IRequestHandler<GetClientsTotalCommandQuery, Result>,
    IRequestHandler<UpdateClientCommand, Result>
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
        client.Active = true;
        await _repo.AddAsync(client);
        await _repo.SaveAsync();
        return CreateResponse(client, "Cliente cadastrado");
    }

    public async Task<Result> Handle(GetClientsTotalCommandQuery request, CancellationToken cancellationToken)
    {
        var clients = new List<ClientViewModel>();
        
        var _clientsDb = await _repo.GetAllAsync(p => p.CompanyId == request.CompanyId);
        
        foreach (var client in _clientsDb)
        {
            clients.Add(new ClientViewModel
            {
                ClientId = client.ClientId,
                Name = client.Name,
                CreateAt = client.CreateAt,
                Active = client.Active,
                TotalCommands = await _repo.GetTotalCommandsAsync(client.ClientId),
            });
        }
        
        return CreateResponse(clients, "Busca realizada");
    }

    public async Task<Result> Handle(UpdateClientCommand request, CancellationToken cancellationToken)
    {
        request.Name = request.Name.ToUpper();

        var client = await _repo.GetById(request.ClientId);
        if (client is null)
            return CreateError("Cliente não encontrado");

        var sameNameClient = await _repo.GetByNameAsync(request.CompanyId, request.Name);
        if (sameNameClient is not null && sameNameClient.ClientId != request.ClientId)
            return CreateError("Cliente já cadastrado com esse nome");

        client.Name = request.Name;
        client.Active = request.Active;
        _repo.Update(client);
        await _repo.SaveAsync();
        return CreateResponse(client, "Cliente atualizado");
    }
}
