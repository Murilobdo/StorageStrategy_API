using Microsoft.EntityFrameworkCore;
using StorageStrategy.Data.Context;
using StorageStrategy.Domain.Repository;
using StorageStrategy.Models;

namespace StorageStrategy.Data.Repository
{
    public class CommandRepository : RepositoryBase<CommandEntity>, ICommandRepository
    {

        public CommandRepository(StorageDbContext context) : base(context)
        {

        }

        public async Task AddItemsAsync(IEnumerable<CommandItem> items)
        {
            await _context.CommandItems.AddRangeAsync(items);
        }

        public async Task<CommandEntity> GetCommandByIdAsync(int commandId, int companyId)
        {
            return await _context.Command
               .Include(p => p.Items)
               .FirstOrDefaultAsync(p => p.CompanyId == companyId && p.CommandId == commandId);
        }

        public async Task<List<CommandEntity>> ToListAsync(int companyId, bool haveEndDate)
        {
            var query = _context.Command
                .Where(p => p.CompanyId == companyId)
                .Include(p => p.Items)
                .AsQueryable();

            if (haveEndDate)
                query = query.Where(p => p.FinalDate != null);

            return await query.ToListAsync();
        }
    }
}
