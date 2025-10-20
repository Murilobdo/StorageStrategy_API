using AutoMapper;
using StorageStrategy.Domain.Commands.Command;
using StorageStrategy.Domain.Repository;
using StorageStrategy.Models;

namespace StorageStrategy.Domain.Handlers.Command;

public class RemoveProductCommandHandler : CommandHandlerBase<RemoveProductCommandCommand>
{
    public RemoveProductCommandHandler(IProductRepository repoProduct, ICommandRepository repoCommand, IEmployeeRepository repoEmployee, IMapper mapper, IClientRepository clientRepo) : base(repoProduct, repoCommand, repoEmployee, mapper, clientRepo)
    {
    }

    public override async Task<Result> Handle(RemoveProductCommandCommand request, CancellationToken cancellationToken)
    {
        var command = await _repoCommand.GetCommandByIdAsync(request.CommandId, request.CompanyId);
        var productRemove = command.Items.FirstOrDefault(p => p.ProductId == request.ProductId);

        var product = await _repoProduct.GetByIdAsync(request.ProductId, request.CompanyId);
        product.Qtd += productRemove.Qtd;
        _repoProduct.Update(product);
        await _repoCommand.SaveAsync();
        await _repoCommand.RemoveCommandItemsAsync(new List<CommandItemEntity>{productRemove});
        await _repoCommand.SaveAsync();
        return CreateResponse(true, "Produto removido com sucesso");
    }
}