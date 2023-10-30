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

        public override async Task<CommandEntity> GetById(int id)
        {
            return await _context.Command.FirstOrDefaultAsync(p => p.CommandId == id);
        }

        public async Task<List<CommandEntity>> ReadCommandsByDateAsync(int companyId, DateTime initialDate, DateTime finalDate, int employeeId)
        {
            var query = _context.Command
                .AsNoTracking()
                .Include(p => p.Items)
                .Include(p => p.Employee)
                .Where(p => p.InitialDate >= initialDate)
                .Where(p => p.CompanyId == companyId)
                .Where(p => p.FinalDate != null && p.FinalDate.Value <= finalDate)
                .AsQueryable();

            if (employeeId > 0)
                query = query.Where(p => p.EmployeeId == employeeId);

            return await query.ToListAsync();
        }

        public async Task<List<CommandEntity>> ReadCommandsByMounthAsync(int companyId, int month, int? employeeId)
        {
            var query = _context.Command
                .AsNoTracking()
                .Include(p => p.Items)
                .Include(p => p.Employee)
                .Where(p => p.CompanyId == companyId)
                .Where(p => p.FinalDate != null && p.FinalDate.Value.Month == month)
                .AsQueryable();

            if (employeeId > 0)
                query = query.Where(p => p.EmployeeId == employeeId);

            return await query.ToListAsync();
        }
    }
}