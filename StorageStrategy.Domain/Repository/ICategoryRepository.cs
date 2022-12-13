using StorageStrategy.Models;

namespace StorageStrategy.Domain.Repository
{
    public interface ICategoryRepository : IRepositoryBase<CategoryEntity>
    {
        Task<CategoryEntity> FindByName(string name);
    }
}
