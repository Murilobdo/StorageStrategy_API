using StorageStrategy.Models;

namespace StorageStrategy.Domain.Repository
{
    public interface ICommandRepository : IRepositoryBase<CommandEntity>
    {
        Task<List<CommandEntity>> ToList(int companyId);
    }
}
