using AutoMapper;
using Microsoft.Extensions.Options;
using StorageStrategy.Domain.Commands.Products;
using StorageStrategy.Domain.Repository;
using StorageStrategy.Models;
using StorageStrategy.Utils.Services;

namespace StorageStrategy.Domain.Handlers.Product;

public class DeleteProductHandle : ProductHandlerBase<DeleteProductCommand>
{
    public DeleteProductHandle(IProductRepository repoProduct, ICategoryRepository repoCategory, IMapper mapper) : base(repoProduct, repoCategory, mapper)
    {
    }

    public override async Task<Result> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid())
            return CreateError(request.GetErros(), "Dados invalidos");

        var product = await _repoProduct.GetByIdAsync(request.ProductId, request.CompanyId);

        if (product is null)
            return CreateError("Produto não encontrado para exclusão.");

        _repoProduct.Delete(product);
        await _repoProduct.SaveAsync();

        return CreateResponse(product, "Produto excluido com sucesso.");
    }
}