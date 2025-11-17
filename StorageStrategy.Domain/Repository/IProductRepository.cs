using StorageStrategy.Models;

namespace StorageStrategy.Domain.Repository
{
    public interface IProductRepository : IRepositoryBase<ProductEntity>
    {
        Task<ProductEntity> FindByName(string name, int companyId);
        Task<List<ProductEntity>> ToList(int companyId, bool active);
        Task<List<ProductEntity>> ToList(int companyId);
        Task<ProductEntity> GetByIdAsync(int productId, int companyId);
        Task AddRange(List<ProductEntity> products);
        Task<decimal> ReadTotalPriceInStokByCompany(int companyId);
        Task<int> QuantityInStockByCompany(int companyId);
        Task<List<ProductEntity>> GetProductsByIds(int companyId, IEnumerable<int> productsIds);
        Task AddStockHistory(StockHistoryEntity stockHistory);
        Task<ProductEntity> FindLastProductByCompany(int requestCompanyId);
    }
}
