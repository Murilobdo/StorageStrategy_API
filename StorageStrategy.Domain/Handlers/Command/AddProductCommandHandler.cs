
using AutoMapper;
using StorageStrategy.Domain.Commands.Command;
using StorageStrategy.Domain.Handlers.Command;
using StorageStrategy.Domain.Repository;
using StorageStrategy.Models;

public class AddProductCommandHandler : CommandHandlerBase<AddProductCommandCommand>
{
    public AddProductCommandHandler(IProductRepository repoProduct, ICommandRepository repoCommand, IEmployeeRepository repoEmployee, IMapper mapper, IClientRepository clientRepo)
        : base(repoProduct, repoCommand, repoEmployee, mapper, clientRepo)
    {
    }

    public override async Task<Result> Handle(AddProductCommandCommand request, CancellationToken cancellationToken)
    {
         if (!request.IsValid())
                return CreateError(request.GetErros(), "Dados invalidos.");
            
            var command = await _repoCommand.GetCommandByIdAsync(request.CommandId, request.CompanyId);
            if (command is null)  
                return CreateError("Comanda não encontrada");

            var productItems = command.Items;
            var productsRequest = request.Items.Select(p => _mapper.Map<CommandItemEntity>(p)).ToList();
            productsRequest.ForEach(p => p.CommandId = command.CommandId);

            var result = await HasProductsInStockCommand(request.Items, productItems, request.CompanyId);

            if (!result.Success)
                return CreateError(result.Errors[0].ErrorMessage);
       
            foreach (var productRequest in productsRequest)
            {
                var product = await _repoProduct.GetByIdAsync(productRequest.ProductId, request.CompanyId);
                if (productRequest.CommandItemId == 0)
                {
                    await AddProductInCommand(product, productRequest);
                    continue;
                }
                
                var productItemDb = productItems.FirstOrDefault(p => p.CommandItemId == productRequest.CommandItemId);
                //2 > 5
                if (productRequest.Qtd > productItemDb.Qtd)
                {
                    var difference = Math.Abs(productRequest.Qtd - productItemDb.Qtd);
                    product.Qtd -= difference;
                    _repoProduct.Update(product);
                    await _repoProduct.SaveAsync();
                }
                
                if(productItemDb.Qtd > productRequest.Qtd)
                {
                    var difference = Math.Abs(productRequest.Qtd - productItemDb.Qtd);
                    product.Qtd += difference;
                    _repoProduct.Update(product);
                    await _repoProduct.SaveAsync();
                }
                
                productItemDb.Qtd = productRequest.Qtd;
                _repoCommand.UpdateCommandItemAsync(productItemDb);
                await _repoCommand.SaveAsync();
                
            }
            
            command.TotalPrice = productsRequest.Sum(p => p.Price * p.Qtd);
            command.TotalCost = productsRequest.Sum(p => p.Cost * p.Qtd);
            _repoCommand.Update(command);
            await _repoCommand.SaveAsync();
            

            return CreateResponse(command, "Comanda atualizada com sucesso.");
    }
    
    private async Task<Result> HasProductsInStockCommand(List<CommandItemBase> items, List<CommandItemEntity> itensDb, int companyId)
    {
        foreach (var itemCommand in items)
        {
            var product = await _repoProduct.GetByIdAsync(itemCommand.ProductId, companyId);
            var itemCommandDb = itensDb.FirstOrDefault(p => p.CommandItemId == itemCommand.CommandItemId);

            if (itemCommandDb is null || (itemCommand.Qtd < itemCommandDb.Qtd))
                continue;
                
            var difference = Math.Abs(itemCommand.Qtd - itemCommandDb.Qtd);
            if (difference > product.Qtd)
            {
                return new Result(new Error($"Quantidade indisponivel em estoque [{product.Name.Trim()}]"));
            }
        }

        return new Result(string.Empty);
    }
    
    private async Task AddProductInCommand(ProductEntity product, CommandItemEntity productRequest)
    {
        product.Qtd -= productRequest.Qtd;
        var newProductItem = new CommandItemEntity(productRequest.CommandId, productRequest.ProductId,
            productRequest.Name, productRequest.Cost, productRequest.Price, productRequest.Qtd);
                
        await _repoCommand.AddItemsAsync(newProductItem);
    }
}