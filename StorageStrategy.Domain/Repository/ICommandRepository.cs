using StorageStrategy.Models;

namespace StorageStrategy.Domain.Repository
{
    public interface ICommandRepository : IRepositoryBase<CommandEntity>
    {
        Task<List<CommandEntity>> ToListAsync(int companyId, bool haveEndDate);
        Task AddItemsAsync(IEnumerable<CommandItem> items);
        Task<CommandEntity> GetCommandByIdAsync(int commandId, int companyId);
        Task RemoveCommandItems(List<CommandItem> items);
        Task<List<CommandEntity>> ReadCommandsForPeriod(int companyId, int initialMonth, int finalMounth = 0);
    }
}
