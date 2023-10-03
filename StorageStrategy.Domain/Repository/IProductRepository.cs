using StorageStrategy.Models;

namespace StorageStrategy.Domain.Repository
{
    public interface IProductRepository : IRepositoryBase<ProductEntity>
    {
        Task<ProductEntity> FindByName(string name, int companyId);
        Task<List<ProductEntity>> ToList(int companyId);
        Task<ProductEntity> GetByIdAsync(int categoryId, int companyId);
        Task AddRange(List<ProductEntity> products);
        Task<decimal> ReadTotalPriceInStokByCompany(int companyId);
        Task<int> QuantityInStockByCompany(int companyId);
    }
}
