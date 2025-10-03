using Microsoft.EntityFrameworkCore;
using StorageStrategy.Data.Context;
using StorageStrategy.Domain.Commands.Report;
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

        public async Task<List<CommandEntity>> ReadFinishCommandsByDateAsync(ReadCommandsBetweenDatesCommand request)
        {
            var query = _context.Command
                .AsNoTracking()
                .Include(p => p.Items)
                    .ThenInclude(p => p.Product)
                .Include(p => p.Payments)
                .Include(p => p.Employee)
                .Include(p => p.Client)
                .Where(p => p.CompanyId == request.CompanyId)
                .Where(p => p.FinalDate != null && p.FinalDate.Value >= request.InitialDate && p.FinalDate.Value <= request.FinalDate)
                .OrderByDescending(p => p.InitialDate)
                .AsQueryable();

            if (request.EmployeeId > 0)
                query = query.Where(p => p.EmployeeId == request.EmployeeId);

            if (request.ClientId > 0)
                query = query.Where(p => p.ClientId == request.ClientId);

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