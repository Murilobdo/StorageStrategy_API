using System.Globalization;
using AutoMapper;
using MediatR;
using StorageStrategy.Domain.Commands.Products;
using StorageStrategy.Domain.Commands.StockHistory;
using StorageStrategy.Domain.Repository;
using StorageStrategy.Models;

namespace StorageStrategy.Domain.Handlers
{
    public class ProductHandler : HandlerBase,
        IRequestHandler<CreateProductCommand, Result>,
        IRequestHandler<UpdateProductCommand, Result>,
        IRequestHandler<DeleteProductCommand, Result>,
        IRequestHandler<ImportProductCommand, Result>,
        IRequestHandler<CreateStockHsitoryCommand, Result>
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
                return CreateError("Ja existe um produto com esse nome");
 
            var category = await _repoCategory.GetById(request.CategoryId);

            if (category is null)
                return CreateError("Categoria não encontrada");

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
                return CreateError("Produto não encontrado para a atualização");

            var sameNameProduct = await _repo.FindByName(request.Name, request.CompanyId);

            if (sameNameProduct is not null && sameNameProduct.ProductId != product.ProductId)
                return CreateError("Ja existe um produto com esse nome");

            if (await _repoCategory.GetById(request.CategoryId) is null)
                return CreateError("Categoria não encontrada");

            product = _mapper.Map<ProductEntity>(request);
            product.StockAlert = request.StockAlert;
            _repo.Clear();
            _repo.Update(product);
            await _repo.SaveAsync();

            return CreateResponse(product, "Produto atualizado com sucesso");
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
            try
            {
                await _repo.CreateTranscationAsync();
                await CreateCategorys(request);

                var products = await CreateProducts(request);
                
                await _repo.CommitAsync();
                return CreateResponse(products, "Produtos importados com sucesso.");
            }
            catch (Exception ex)
            {
                await _repo.RollbackAsync();
                return CreateError(new Result("", ex.Message));
            }
        }

        private async Task<List<ProductEntity>> CreateProducts(ImportProductCommand request)
        {
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
                        companyId: request.CompanyId,
                        taxing: product.Taxing
                    ));
                }
            }

            await _repo.AddRange(products);
            await _repo.SaveAsync();

            return products;
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

        private decimal ConvertMoney(string value)
        {
            value = value.Trim().Replace("R$ ", "").Replace("R$", "");
            value = value.Replace(".", ",");
            decimal returnValue = Convert.ToDecimal(value, new System.Globalization.CultureInfo("pt-BR"));
            return returnValue;
        }

        public async Task<Result> Handle(CreateStockHsitoryCommand request, CancellationToken cancellationToken)
        {
            if(!request.IsValid())
                return CreateError(request.GetErros(), "Dados invalidos");

            StockHistoryEntity stockHistory = new(request.CompanyId, request.Products);

            var productsIds = stockHistory.Products.Select(p => p.ProductId);
            var productsToUpdate = await _repo.GetProductsByIds(request.CompanyId, productsIds);
            await _repo.AddStockHistory(stockHistory);
            await _repo.SaveAsync();

            foreach (var product in productsToUpdate)
            {
                var productHistory = stockHistory.Products.FirstOrDefault(p => p.ProductId == product.ProductId);
                product.Qtd += productHistory.Quantity;
                product.Taxing = productHistory.Taxing;
                product.Cost = productHistory.Cost;
                product.Price = productHistory.Price;
                _repo.Update(product);
            }

            await _repo.SaveAsync();

            return CreateResponse(stockHistory, "Estoque atualizado com sucesso.");
        }
    }
}
