using StorageStrategy.Models;

namespace StorageStrategy.Domain.Repository
{
    public interface IReportRepository : IRepositoryBase<CommandEntity>
    {
        Task<List<CommandEntity>> ReadCommandsByDateAsync(int companyId, DateTime initialDate, DateTime finalDate, int employeeId);
        Task<List<CommandEntity>> ReadCommandsByMounthAsync(int companyId, int month, int? employeeId);
    }
}