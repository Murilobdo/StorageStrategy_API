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

        public async Task AddItemsAsync(IEnumerable<CommandItemEntity> items)
        {
            await _context.CommandItems.AddRangeAsync(items);
        }

        public override async Task<CommandEntity> GetById(int id)
        {
            return await _context.Command.FirstOrDefaultAsync(p => p.CommandId == id);
        }

        public async Task<CommandEntity> GetCommandByIdAsync(int commandId, int companyId)
        {
            return await _context.Command
               .Include(p => p.Items)
               .FirstOrDefaultAsync(p => p.CompanyId == companyId && p.CommandId == commandId);
        }

        public async Task<List<CommandItemEntity>> ReadCommandsForDaysAsync(int companyId, int day)
        {
            var result = await _context.Command
                .AsNoTracking()
                .Where(p => p.FinalDate != null && p.FinalDate.Value.Day == day)
                .SelectMany(p => p.Items)
                .ToListAsync();

            return result;
        }

        public async Task<List<CommandEntity>> ReadCommandsForPeriodAsync(int companyId, int initialMonth, int finalMounth = 0)
        {
            var query =  _context.Command
                            .AsNoTracking()
                            .Where(p => p.FinalDate != null)
                            .Where(p => p.InitialDate.Month == initialMonth)
                            .AsQueryable();

            if (finalMounth > 0)
                query = query.Where(p => p.FinalDate.Value.Month == finalMounth);
                            
            return await query.ToListAsync();
        }

        public async Task RemoveCommandItemsAsync(List<CommandItemEntity> items)
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
