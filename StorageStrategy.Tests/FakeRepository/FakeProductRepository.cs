using StorageStrategy.Domain.Commands.Products;
using StorageStrategy.Domain.Repository;
using StorageStrategy.Models;
using System.ComponentModel.Design;
using System.Xml.Linq;

namespace StorageStrategy.Tests.FakeRepository
{
    public class FakeProductRepository : IProductRepository
    {
        public List<ProductEntity> products;
        public FakeCategoryRepository _repoCategory;

        public CreateProductCommand CreateProductCommand;
        public UpdateProductCommand UpdateProductCommand;
        public DeleteProductCommand DeleteProductCommand;

        public FakeProductRepository()
        {
            _repoCategory = new FakeCategoryRepository();

            products = new List<ProductEntity>
            {
                new ProductEntity(productId: 1, name: "Produto 1", cost: 2.5m, price: 5.75m, qtd: 20, stockAlert: 2, categoryId: _repoCategory.categorys[0].CategoryId, companyId:1, taxing:0),
                new ProductEntity(productId: 2, name: "Produto 2", cost: 2.5m, price: 5.75m, qtd: 20, stockAlert: 2, categoryId: _repoCategory.categorys[1].CategoryId, companyId:1, taxing:0),
                new ProductEntity(productId: 3, name: "Produto 3", cost: 2.5m, price: 5.75m, qtd: 20, stockAlert: 2, categoryId: _repoCategory.categorys[2].CategoryId, companyId:1, taxing:0),
                new ProductEntity(productId: 4, name: "Produto 4", cost: 2.5m, price: 5.75m, qtd: 20, stockAlert: 2, categoryId: _repoCategory.categorys[3].CategoryId, companyId:1, taxing:0),
            };

            CreateProductCommand = new(name: "Coca Cola", cost:4.91m, price: 10m, qtd: 30, stockAlert: 10, isActive: true, categoryId: _repoCategory.categorys[0].CategoryId, companyId: 1, taxing: 1);
            UpdateProductCommand = new(productId: 1, name: "Coca Cola", cost:4.91m, price: 10m, qtd: 30, stockAlert: 10, isActive: true, categoryId: _repoCategory.categorys[0].CategoryId, companyId: 1, taxing: 1 );
            DeleteProductCommand = new(productId: 1, companyId: 1);
        }

        public Task AddAsync(ProductEntity model)
        {
            products.Add(model);
            return Task.CompletedTask;
        }

        public Task AddRange(List<ProductEntity> products)
        {
            products.AddRange(products);
            return Task.CompletedTask;
        }

        public Task CommitAsync()
        {
            return Task.CompletedTask;
        }

        public Task CreateTranscationAsync()
        {
            return Task.CompletedTask;
        }

        public void Delete(ProductEntity id)
        {
        }

        public Task<ProductEntity> FindByName(string name, int companyId)
        {
            return Task.FromResult(products.FirstOrDefault(p => p.Name.Equals(name) && p.CompanyId == companyId));
        }
        public Task<ProductEntity> GetById(int id)
        {
            return Task.FromResult(products.FirstOrDefault(p => p.ProductId == id)) ;
        }

        public Task<ProductEntity> GetByIdAsync(int productId, int companyId)
        {
            return Task.FromResult(products.FirstOrDefault(p => p.ProductId == productId && p.CompanyId == companyId));
        }

        public Task<int> QuantityInStockByCompany(int companyId)
        {
            var totalQtd = products.Where(p => p.CompanyId == companyId)
                .Sum(p => p.Qtd);

            return Task.FromResult(totalQtd);
        }

        public Task<decimal> ReadTotalPriceInStokByCompany(int companyId)
        {
            var totalPrice = products.Where(p => p.CompanyId == companyId)
                            .Sum(p => p.Price);

            return Task.FromResult(totalPrice);
        }

        public void RemoveRange(ProductEntity model)
        {
        }

        public Task RollbackAsync()
        {
            return Task.CompletedTask;
        }

        public void Save()
        {
        }

        public Task SaveAsync()
        {
            return Task.CompletedTask;
        }

        public Task<List<ProductEntity>> ToList(int companyId)
        {
            return Task.FromResult(products.Where(p => p.CompanyId == companyId).ToList());
        }

        public void Update(ProductEntity model)
        {
        }
    }
}
