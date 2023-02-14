using StorageStrategy.Models;

namespace StorageStrategy.Domain.Repository
{
    public interface ICommandRepository : IRepositoryBase<CommandEntity>
    {
        Task<List<CommandEntity>> ToListAsync(int companyId, bool haveEndDate);
        Task AddItemsAsync(IEnumerable<CommandItemEntity> items);
        Task<CommandEntity> GetCommandByIdAsync(int commandId, int companyId);
        Task RemoveCommandItemsAsync(List<CommandItemEntity> items);
        Task<List<CommandEntity>> ReadCommandsForPeriodAsync(int companyId, int initialMonth, int finalMounth = 0);
        Task<List<CommandItemEntity>> ReadCommandsForDaysAsync(int companyId, int day);
    }
}
