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

        public async Task<List<CommandEntity>> ReadCommandsForPeriod(int companyId, int initialMonth, int finalMounth = 0)
        {
            var query =  _context.Command
                            .AsNoTracking()
                            .Where(p => p.FinalDate != null)
                            .Where(p => p.InitialDate.Month == initialMonth)
                            .AsQueryable();

            if (finalMounth > 0)
                query = query.Where(p => p.FinalDate.Value.Month == finalMounth);
                            
            return query.ToList();
        }

        public async Task RemoveCommandItems(List<CommandItem> items)
        {
            _context.CommandItems.RemoveRange(items);
            await _context.SaveChangesAsync();
        }

        public async Task<List<CommandEntity>> ToListAsync(int companyId, bool haveEndDate)
        {
            if (haveEndDate)
                return await _context.Command
                    .Where(p => p.CompanyId == companyId)
                    .Include(p => p.Items)
                    .Where(p => p.FinalDate != null)
                    .ToListAsync();
            else
                return await _context.Command
                    .Where(p => p.CompanyId == companyId)
                    .Include(p => p.Items)
                    .Where(p => p.FinalDate == null)
                    .ToListAsync();
        }
    }
}
