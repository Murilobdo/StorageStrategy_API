using StorageStrategy.Models;

namespace StorageStrategy.Domain.Repository
{
    public interface IProductRepository : IRepositoryBase<ProductEntity>
    {
        Task<ProductEntity> FindByName(string name, int companyId);
        Task<List<ProductEntity>> ToList(int companyId, bool active);
        Task<ProductEntity> GetByIdAsync(int productId, int companyId);
        Task AddRange(List<ProductEntity> products);
        Task<decimal> ReadTotalPriceInStokByCompany(int companyId);
        Task<int> QuantityInStockByCompany(int companyId);
        Task<decimal> ReadSalesOfTheDay(int companyId, int month, int day);
    }
}
