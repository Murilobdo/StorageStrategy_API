using AutoMapper;
using StorageStrategy.Domain.Commands.Command;
using StorageStrategy.Domain.Repository;
using StorageStrategy.Models;

namespace StorageStrategy.Domain.Handlers.Command;

public class ListCommandsHandler : CommandHandlerBase<ListCommandsQuery>
{
    public ListCommandsHandler(IProductRepository repoProduct, ICommandRepository repoCommand, IEmployeeRepository repoEmployee, IMapper mapper, IClientRepository clientRepo) : base(repoProduct, repoCommand, repoEmployee, mapper, clientRepo)
    {
    }

    public override async Task<Result> Handle(ListCommandsQuery request, CancellationToken cancellationToken)
    {
        if (request.CompanyId == 0)
            return CreateError("Dados invalidos");
        
        List<CreateCommandCommand> result = new();
        var commands = await _repoCommand.ToListAsync(request.CompanyId, request.HaveEndDate);
        commands = commands.OrderByDescending(p => p.InitialDate).ToList();

        commands.ForEach(_command =>
        {
            CreateCommandCommand command = new CreateCommandCommand(_command);
            result.Add(command);
        });

        return CreateResponse(result, "Busca realizada");
    }
}