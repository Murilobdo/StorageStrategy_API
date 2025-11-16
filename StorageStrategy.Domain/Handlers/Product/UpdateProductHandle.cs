using AutoMapper;
using StorageStrategy.Domain.Commands.Products;
using StorageStrategy.Domain.Repository;
using StorageStrategy.Models;

namespace StorageStrategy.Domain.Handlers.Product;

public class UpdateProductHandle : ProductHandlerBase<UpdateProductCommand>
{
    public UpdateProductHandle(IProductRepository repoProduct, ICategoryRepository repoCategory, IMapper mapper) : base(repoProduct, repoCategory, mapper)
    {
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

        return CreateResponse(product, "Produto atualizado com sucesso");
    }
}