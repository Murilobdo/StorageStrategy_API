using AutoMapper;
using StorageStrategy.Domain.Commands.Products;
using StorageStrategy.Domain.Repository;
using StorageStrategy.Domain.Services.MinioStorage;
using StorageStrategy.Models;

namespace StorageStrategy.Domain.Handlers.Product;

public class CreateProductHandle : ProductHandlerBase<CreateProductCommand>
{
    private IStorageFile _storage;
    private ICompanyRepository _repoCompany;
    
    public CreateProductHandle(
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

    public override async Task<Result> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid())
            return CreateError(request.GetErros(), "Dados invalidos");

        var product = await _repoProduct.FindByName(request.Name, request.CompanyId);
                
        if (product is not null)
            return CreateError("Ja existe um produto com esse nome");
 
        var category = await _repoCategory.GetById(request.CategoryId);

        if (category is null)
            return CreateError("Categoria não encontrada");

        product = _mapper.Map<ProductEntity>(request);
        product.StockAlert = request.StockAlert;
        product.Taxing = request.Taxing;
        await _repoProduct.AddAsync(product);
        await _repoProduct.SaveAsync();

        request.ProductId = product.ProductId;
        await UploadPhotoAsync(request);

        return CreateResponse(product, "Produto cadastrado com sucesso.");
    }
    
    private async Task UploadPhotoAsync(CreateProductCommand request)
    {
        var company = await _repoCompany.GetById(request.CompanyId);
        if (request.PhotoUrl is null || request.PhotoUrl.Length == 0 || company is null)
            return;

        var path = await _storage.UploadProductPhotoAsync(request.PhotoUrl, company.Name, request.ProductId);

        var product = await _repoProduct.GetById(request.ProductId);
        product.PhotoUrl = path;

        _repoProduct.Update(product);
        await _repoProduct.SaveAsync();
    }
}