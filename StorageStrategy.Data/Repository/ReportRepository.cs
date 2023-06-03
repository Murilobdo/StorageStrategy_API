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

        public async Task<List<CommandEntity>> ReadCommandsByDateAsync(int companyId, DateTime initialDate, DateTime finalDate, int employeeId)
        {
            {
                var query = _context.Command
                    .Include(p => p.Items)
                    .Include(p => p.Employee)
                    .Where(p => p.InitialDate >= initialDate)
                    .Where(p => p.InitialDate <= finalDate)
                    .AsQueryable();

                if (employeeId > 0)
                    query = query.Where(p => p.EmployeeId == employeeId);

                return await query.ToListAsync();
            }
        }
    }
}