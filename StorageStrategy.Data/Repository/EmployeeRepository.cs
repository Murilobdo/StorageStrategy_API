using Microsoft.EntityFrameworkCore;
using StorageStrategy.Data.Context;
using StorageStrategy.Domain.Repository;
using StorageStrategy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageStrategy.Data.Repository
{
    public class EmployeeRepository : RepositoryBase<EmployeeEntity>, IEmployeeRepository
    {
        private StorageDbContext _context;

        public EmployeeRepository(StorageDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<EmployeeEntity> FindByName(string name, int companyId)
        {
            return await _context.Employee.FirstOrDefaultAsync(p => p.Name == name && p.CompanyId == companyId);
        }

        public async Task<EmployeeEntity> GetByIdAsync(int employeeId, int companyId)
        {
            var result = await _context.Employee.FirstOrDefaultAsync(p => p.EmployeeId == employeeId && p.CompanyId == companyId);
            _context.ChangeTracker.Clear();
            return result;
        }

        public async Task<List<EmployeeEntity>> ToList(int companyId)
        {
            return await _context.Employee.Where(p => p.CompanyId == companyId).ToListAsync();
        }
    }
}
