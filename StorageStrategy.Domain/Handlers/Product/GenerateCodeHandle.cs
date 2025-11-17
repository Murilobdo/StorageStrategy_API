using AutoMapper;
using StorageStrategy.Domain.Commands.Products;
using StorageStrategy.Domain.Repository;
using StorageStrategy.Models;

namespace StorageStrategy.Domain.Handlers.Product;

public class GenerateCodeHandle : ProductHandlerBase<GenerateCodeCommand>
{
    public GenerateCodeHandle(IProductRepository repoProduct, ICategoryRepository repoCategory, IMapper mapper) : base(repoProduct, repoCategory, mapper)
    {
    }

    public override async Task<Result> Handle(GenerateCodeCommand request, CancellationToken cancellationToken)
    {
        var lastProduct = await _repoProduct.FindLastProductByCompany(request.CompanyId);
        request.NewCode = Convert.ToString(lastProduct is null ? 1 : Convert.ToInt32(lastProduct.Code) + 1);
        return CreateResponse(request, "Produto excluido com sucesso.");
    }
}