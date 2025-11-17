using AutoMapper;
using Microsoft.Extensions.Options;
using StorageStrategy.Domain.Commands.Products;
using StorageStrategy.Domain.Commands.StockHistory;
using StorageStrategy.Domain.Repository;
using StorageStrategy.Models;
using StorageStrategy.Utils.Services;

namespace StorageStrategy.Domain.Handlers.Product;

public class StockHistoryProductHandle : ProductHandlerBase<CreateStockHistoryCommand>
{
    public StockHistoryProductHandle(IProductRepository repoProduct, ICategoryRepository repoCategory, IMapper mapper) : base(repoProduct, repoCategory, mapper)
    {
    }

    public override async Task<Result> Handle(CreateStockHistoryCommand request, CancellationToken cancellationToken)
    {
        if(!request.IsValid())
            return CreateError(request.GetErros(), "Dados invalidos");

        StockHistoryEntity stockHistory = new(request.CompanyId, request.Products);

        var productsIds = stockHistory.Products.Select(p => p.ProductId);
        var productsToUpdate = await _repoProduct.GetProductsByIds(request.CompanyId, productsIds);
        await _repoProduct.AddStockHistory(stockHistory);
        await _repoProduct.SaveAsync();

        foreach (var product in productsToUpdate)
        {
            var productHistory = stockHistory.Products.FirstOrDefault(p => p.ProductId == product.ProductId);
            product.Qtd += productHistory.Quantity;
            product.Taxing = productHistory.Taxing;
            product.Cost = productHistory.Cost;
            product.Price = productHistory.Price;
            _repoProduct.Update(product);
        }

        await _repoProduct.SaveAsync();

        return CreateResponse(stockHistory, "Estoque atualizado com sucesso.");
    }
}