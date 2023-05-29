using StorageStrategy.Models;

namespace StorageStrategy.Domain.Repository
{
    public interface IReportRepository : IRepositoryBase<CommandEntity>
    {
        Task<List<CommandEntity>> ReadCommandsByMounthAsync(int companyId, int month);
    }
}