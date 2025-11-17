using AutoMapper;
using Microsoft.Extensions.Options;
using StorageStrategy.Domain.Commands.Products;
using StorageStrategy.Domain.Repository;
using StorageStrategy.Domain.Services.MinioStorage;
using StorageStrategy.Models;
using StorageStrategy.Utils.Services;

namespace StorageStrategy.Domain.Handlers.Product;

public class UpdateProductHandle : ProductHandlerBase<UpdateProductCommand>
{
    private IStorageFile _storage;
    private ICompanyRepository _repoCompany;
    
    public UpdateProductHandle(
        IProductRepository repoProduct, 
        ICategoryRepository repoCategory, 
        IMapper mapper,
        IStorageFile storage,
        ICompanyRepository repoCompany
    ) : base(repoProduct, repoCategory, mapper)
    {
        _storage = storage;
        _repoCompany = repoCompany;
    }

    public override async Task<Result> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid())
            return CreateError(request.GetErros(), "Dados invalidos");

        var product = await _repoProduct.GetByIdAsync(request.ProductId, request.CompanyId);

        if (product is null)
            return CreateError("Produto não encontrado para a atualização");

        var sameNameProduct = await _repoProduct.FindByName(request.Name, request.CompanyId);

        if (sameNameProduct is not null && sameNameProduct.ProductId != product.ProductId)
            return CreateError("Ja existe um produto com esse nome");

        if (await _repoCategory.GetById(request.CategoryId) is null)
            return CreateError("Categoria não encontrada");

        product = _mapper.Map<ProductEntity>(request);
        product.StockAlert = request.StockAlert;
        _repoProduct.Clear();
        _repoProduct.Update(product);
        await _repoProduct.SaveAsync();
        
        await UploadPhotoIfHasChangedAsync(request, product);

        return CreateResponse(product, "Produto atualizado com sucesso");
    }
    
    private async Task UploadPhotoIfHasChangedAsync(UpdateProductCommand request, ProductEntity product)
    {
        if (!request.PhotoUrl.Contains("base64"))
            return;
        
        if (request.PhotoUrl is null || request.PhotoUrl.Length == 0)
            return;
        
        await UploadPhotoAsync(request);
    }
    
    
    
    private async Task UploadPhotoAsync(UpdateProductCommand request)
    {
        var company = await _repoCompany.GetById(request.CompanyId);
        if (request.PhotoUrl is null || request.PhotoUrl.Length == 0 || company is null)
            return;

        var path = await _storage.UploadProductPhotoAsync(request.PhotoUrl, company.Name, request.ProductId);

        var product = await _repoProduct.GetById(request.ProductId);
        product.PhotoUrl = $"{path}";

        _repoProduct.Update(product);
        await _repoProduct.SaveAsync();
    }
}