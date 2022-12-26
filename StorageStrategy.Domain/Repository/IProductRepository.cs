using StorageStrategy.Models;

namespace StorageStrategy.Domain.Repository
{
    public interface IProductRepository : IRepositoryBase<ProductEntity>
    {
        Task<ProductEntity> FindByName(string name, int companyId);
        Task<List<ProductEntity>> ToList(int companyId);
        public Task<ProductEntity> GetByIdAsync(int categoryId, int companyId);
    }
}
