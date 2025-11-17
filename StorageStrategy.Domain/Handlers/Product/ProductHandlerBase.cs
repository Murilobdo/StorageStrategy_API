using AutoMapper;
using MediatR;
using Microsoft.Extensions.Options;
using StorageStrategy.Domain.Repository;
using StorageStrategy.Models;
using StorageStrategy.Utils.Services;

namespace StorageStrategy.Domain.Handlers.Product;

public class ProductHandlerBase<T> : HandlerBase, IRequestHandler<T, Result> where T : IRequest<Result>
{
    protected IProductRepository _repoProduct;
    protected ICategoryRepository _repoCategory;
    protected IMapper _mapper;
    protected IOptions<AppSettings> _options;
    
    public ProductHandlerBase(
        IProductRepository repoProduct,
        ICategoryRepository repoCategory,
        IMapper mapper
    ) {
        _repoProduct = repoProduct;
        _repoCategory = repoCategory;
        _mapper = mapper;
    }
    public virtual Task<Result> Handle(T request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}