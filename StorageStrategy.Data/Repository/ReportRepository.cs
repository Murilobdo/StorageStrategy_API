using Microsoft.EntityFrameworkCore;
using StorageStrategy.Data.Context;
using StorageStrategy.Domain.Repository;
using StorageStrategy.Models;

namespace StorageStrategy.Data.Repository
{
    public class ReportRepository : RepositoryBase<CommandEntity>, IReportRepository
    {
        public ReportRepository(StorageDbContext context) : base(context)
        {
        }

        public async Task<List<CommandEntity>> ReadCommandsByMounthAsync(int companyId, int month)
        {
            var commands = await _context.Command
                .Include(p => p.Items)
                .Where(p => p.CompanyId == companyId)
                .Where(p => p.FinalDate.HasValue && p.FinalDate.Value.Month == month)
                .ToListAsync();
        
            return commands;
        }
    }
}