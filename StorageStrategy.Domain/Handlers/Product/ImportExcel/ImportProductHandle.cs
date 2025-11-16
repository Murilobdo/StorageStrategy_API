using AutoMapper;
using StorageStrategy.Domain.Commands.Products;
using StorageStrategy.Domain.Repository;
using StorageStrategy.Models;

namespace StorageStrategy.Domain.Handlers.Product.ImportExcel;

public class ImportProductHandle: ProductHandlerBase<ImportProductCommand>
{
    public ImportProductHandle(IProductRepository repoProduct, ICategoryRepository repoCategory, IMapper mapper) : base(repoProduct, repoCategory, mapper)
    {
    }
    
    public async Task<Result> Handle(ImportProductCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _repoProduct.CreateTranscationAsync();
            await CreateCategorys(request);

            var products = await CreateProducts(request);
                
            await _repoProduct.CommitAsync();
            return CreateResponse(products, "Produtos importados com sucesso.");
        }
        catch (Exception ex)
        {
            await _repoProduct.RollbackAsync();
            return CreateError(new Result("", ex.Message));
        }
    }
    
    private async Task<List<ProductEntity>> CreateProducts(ImportProductCommand request)
    {
        List<ProductEntity> products = new();
        foreach (var product in request.Products)
        {
            var category = await _repoCategory.FindByName(product.Category, request.CompanyId);

            if (await _repoProduct.FindByName(product.Name, request.CompanyId) is null)
            {
                products.Add(new ProductEntity(
                    name: product.Name,
                    cost: ConvertMoney(product.Cost),
                    price: ConvertMoney(product.Price),
                    qtd: Convert.ToInt32(product.Qtd),
                    stockAlert: Convert.ToInt32(product.StockAlert),
                    categoryId: category.CategoryId,
                    companyId: request.CompanyId,
                    taxing: product.Taxing
                ));
            }
        }

        await _repoProduct.AddRange(products);
        await _repoProduct.SaveAsync();

        return products;
    }
    
    private decimal ConvertMoney(string value)
    {
        value = value.Trim().Replace("R$ ", "").Replace("R$", "");
        value = value.Replace(".", ",");
        decimal returnValue = Convert.ToDecimal(value, new System.Globalization.CultureInfo("pt-BR"));
        return returnValue;
    }
    
    private async Task CreateCategorys(ImportProductCommand request)
    {
        var categorys = request.Products.Select(p => new CategoryEntity(p.Category, request.CompanyId));

        foreach (var category in categorys)
        {
            if (await _repoCategory.FindByName(category.Name, request.CompanyId) is null)
                await _repoCategory.AddAsync(category);
        }
        await _repoCategory.SaveAsync();

    }
}