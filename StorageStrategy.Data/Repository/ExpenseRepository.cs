using Microsoft.EntityFrameworkCore;
using StorageStrategy.Data.Context;
using StorageStrategy.Domain.Repository;
using StorageStrategy.Models;

namespace StorageStrategy.Data.Repository
{
    public class ExpenseRepository : RepositoryBase<ExpenseEntity>, IExpenseRepository
    {
        public ExpenseRepository(StorageDbContext context) : base(context)
        {
        }

        public async Task AddExpenseTypeAsync(ExpensesTypeEntity expensesType)
        {
            await _context.ExpensesType.AddAsync(expensesType);
        }

        public void DeleteExpenseType(ExpensesTypeEntity expensesType)
        {
            _context.ExpensesType.Remove(expensesType);
        }

        public override async Task<ExpenseEntity> GetById(int id)
        {
            return await _context.Expenses.FirstOrDefaultAsync(p => p.ExpenseId == id);
        }

        public Task<ExpenseEntity> GetExpensesByIdAsync(int expensesId, int companyId)
        {
            return _context.Expenses.FirstOrDefaultAsync(p => p.CompanyId== companyId && p.ExpenseId == expensesId);
        }

        public Task<ExpensesTypeEntity> GetExpensesTypeByDescriptionAsync(string description, int companyId)
        {
            return _context.ExpensesType.FirstOrDefaultAsync(p => p.CompanyId == companyId && p.Description.ToLower().Trim() == description.ToLower().Trim());
        }

        public Task<ExpensesTypeEntity> GetExpensesTypeByIdAsync(int expensesTypeId, int companyId)
        {
            return _context.ExpensesType.FirstOrDefaultAsync(p => p.CompanyId == companyId && p.ExpenseTypeId == expensesTypeId);
        }

        public async Task<List<ExpensesTypeEntity>> ReadExpensesTypeAsync(int companyId)
        {
            return _context.ExpensesType.Where(p => p.CompanyId == companyId).ToList();
        }

        public async Task<List<ExpenseEntity>> ReadTotalExpensesByMonth(int companyId, int month)
        {
            return await _context.Expenses
                .AsNoTracking()
                .Where(p => p.CompanyId == companyId)
                .Where(p => p.CreateAt.Month == month)
                .ToListAsync();

            throw new NotImplementedException();
        }

        public async Task<List<ExpenseEntity>> ToList(int companyId)
        {
            return await _context.Expenses.Where(p => p.CompanyId == companyId).ToListAsync();
        }

        public void UpdateExpenseType(ExpensesTypeEntity expensesType)
        {
            _context.ExpensesType.Update(expensesType);
        }
    }
}
