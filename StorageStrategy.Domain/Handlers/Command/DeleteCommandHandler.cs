using AutoMapper;
using StorageStrategy.Domain.Commands.Command;
using StorageStrategy.Domain.Repository;
using StorageStrategy.Models;

namespace StorageStrategy.Domain.Handlers.Command;

public class DeleteCommandHandler : CommandHandlerBase<DeleteCommandCommand>
{
    public DeleteCommandHandler(IProductRepository repoProduct, ICommandRepository repoCommand, IEmployeeRepository repoEmployee, IMapper mapper, IClientRepository clientRepo) : base(repoProduct, repoCommand, repoEmployee, mapper, clientRepo)
    {
    }

    public override async Task<Result> Handle(DeleteCommandCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid())
            return CreateError(request.GetErros(), "Dados invalidos");

        var command = await _repoCommand.GetCommandByIdAsync(request.CommandId, request.CompanyId);

        if (command.Items.Count > 0)
            return CreateError("Não é possivel excluir uma comanda com produtos");

        _repoCommand.Delete(command);
        await _repoCommand.SaveAsync();

        return CreateResponse(command, "Comanda removida com sucesso!");
    }
}