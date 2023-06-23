using System.Globalization;
using AutoMapper;
using MediatR;
using StorageStrategy.Domain.Commands.Products;
using StorageStrategy.Domain.Repository;
using StorageStrategy.Models;

namespace StorageStrategy.Domain.Handlers
{
    public class ProductHandler : HandlerBase,
        IRequestHandler<CreateProductCommand, Result>,
        IRequestHandler<UpdateProductCommand, Result>,
        IRequestHandler<DeleteProductCommand, Result>,
        IRequestHandler<ImportProductCommand, Result>
    {
        private IProductRepository _repo;
        private ICategoryRepository _repoCategory;
        private IMapper _mapper;

        public ProductHandler(IProductRepository repo, ICategoryRepository repoCategory, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
            _repoCategory = repoCategory;
        }

        public async Task<Result> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
                return CreateError(request.GetErros(), "Dados invalidos");

            var product = await _repo.FindByName(request.Name, request.CompanyId);

            if (product is not null)
                return CreateError($"Ja existe um produto com esse nome {product.Name}");

            var category = await _repoCategory.GetByIdAsync(request.CategoryId, request.CompanyId);

            if (category is null)
            {
                return CreateError("Categoria não encontrada");
            }

            product = _mapper.Map<ProductEntity>(request);

            await _repo.AddAsync(product);
            await _repo.SaveAsync();

            return CreateResponse(product, "Produto cadastrado com sucesso.");
        }

        public async Task<Result> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
                return CreateError(request.GetErros(), "Dados invalidos");

            var product = await _repo.GetByIdAsync(request.ProductId, request.CompanyId);

            if (product is null)
                return CreateError("Produto não encontrado para a atualização.");

            var category = await _repoCategory.GetByIdAsync(request.CategoryId, request.CompanyId);

            if (category is null)
                return CreateError("Categoria não encontrada");

            product = _mapper.Map<ProductEntity>(request);

            _repo.Update(product);
            await _repo.SaveAsync();

            return CreateResponse(product, "Produto atualizado com sucesso.");
        }

        public async Task<Result> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
                return CreateError(request.GetErros(), "Dados invalidos");

            var product = await _repo.GetByIdAsync(request.ProductId, request.CompanyId);

            if (product is null)
                return CreateError("Produto não encontrado para exclusão.");

            _repo.Delete(product);
            await _repo.SaveAsync();

            return CreateResponse(product, "Produto excluido com sucesso.");
        }

        public async Task<Result> Handle(ImportProductCommand request, CancellationToken cancellationToken)
        {
            var categorys = request.Products.Select(p => new CategoryEntity(p.Category, request.CompanyId));

            foreach (var category in categorys)
            {
                if (await _repoCategory.FindByName(category.Name, request.CompanyId) is null)
                    await _repoCategory.AddAsync(category);
            }
            await _repoCategory.SaveAsync();

            List<ProductEntity> products = new();
            foreach (var product in request.Products)
            {
                var category = await _repoCategory.FindByName(product.Category, request.CompanyId);

                if (await _repo.FindByName(product.Name, request.CompanyId) is null)
                {
                    products.Add(new ProductEntity(
                        name: product.Name,
                        cost: ConvertMoney(product.Cost),
                        price: ConvertMoney(product.Price),
                        qtd: Convert.ToInt32(product.Qtd),
                        stockAlert: Convert.ToInt32(product.StockAlert),
                        categoryId: category.CategoryId,
                        companyId: request.CompanyId
                    ));
                }
            }

            await _repo.AddRange(products);

            return CreateResponse(products, "Produtos importados com sucesso.");
        }

        private decimal ConvertMoney(string value)
        {
            value = value.Trim().Replace("R$ ", "").Replace("R$", "");
            return Convert.ToDecimal(value);
        }
    }
}
