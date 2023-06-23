using StorageStrategy.Models;

namespace StorageStrategy.Domain.Repository
{
    public interface ICategoryRepository : IRepositoryBase<CategoryEntity>
    {
        Task<CategoryEntity> FindByName(string name, int companyId);
        Task<List<CategoryEntity>> ToList(int companyId);
        Task<CategoryEntity> GetByIdAsync(int categoryId, int companyId);
        Task AddRange(IEnumerable<CategoryEntity> categorys);
    }
}
