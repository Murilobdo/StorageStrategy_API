using StorageStrategy.Domain.Commands.Report;
using StorageStrategy.Models;

namespace StorageStrategy.Domain.Repository
{
    public interface IReportRepository : IRepositoryBase<CommandEntity>
    {
        Task<List<CommandEntity>> ReadCommandsByDateAsync(ReadCommandsBetweenDatesCommand request);
        Task<List<CommandEntity>> ReadCommandsByMounthAsync(int companyId, int month, int? employeeId);
    }
}