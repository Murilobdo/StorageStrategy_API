using AutoMapper;
using StorageStrategy.Domain.Commands.Products;
using StorageStrategy.Domain.Repository;
using StorageStrategy.Domain.Services.MinioStorage;
using StorageStrategy.Models;

namespace StorageStrategy.Domain.Handlers.Product;

public class CreateProductHandle : ProductHandlerBase<CreateProductCommand>
{
    private IStorageFile _storage;
    
    public CreateProductHandle(
        IProductRepository repoProduct, 
        ICategoryRepository repoCategory, 
        IMapper mapper,
        IStorageFile storage
    ) : base(repoProduct, repoCategory, mapper)
    {
        _storage = storage;
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

        await _repoProduct.AddAsync(product);
        await _repoProduct.SaveAsync();

        await UploadPhotoAsync(request);

        return CreateResponse(product, "Produto cadastrado com sucesso.");
    }
    
    private async Task UploadPhotoAsync(CreateProductCommand request)
    {
        if (request.PhotoUrl is null || request.PhotoUrl.Length == 0)
            return;

        var path = await _storage.UploadProductPhotoAsync(request.PhotoUrl, request.CompanyId, request.ProductId);

        var product = await _repoProduct.GetById(request.ProductId);
        product.PhotoUrl = path;

        _repoProduct.Update(product);
        await _repoProduct.SaveAsync();
    }
    
    
}