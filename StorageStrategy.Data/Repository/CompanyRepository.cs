using Microsoft.EntityFrameworkCore;
using StorageStrategy.Data.Context;
using StorageStrategy.Domain.Repository;
using StorageStrategy.Models;

namespace StorageStrategy.Data.Repository
{
    public class CompanyRepository : RepositoryBase<CompanyEntity>, ICompanyRepository
    {
        public CompanyRepository(StorageDbContext context) : base(context)
        {

        }

        public async Task CreateCompanyExpenseCategorys(List<ExpensesTypeEntity> expensesTypes)
        {
            await _context.ExpensesType.AddRangeAsync(expensesTypes);
        }

        public override async Task<CompanyEntity> GetById(int companyId)
        {
            return await _context.Company.FirstOrDefaultAsync(company => company.CompanyId == companyId);
        }

        public async Task<List<CompanyEntity>> ToList()
        {
            return await _context.Company.ToListAsync();
        }

       
    }
}
