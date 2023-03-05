using Microsoft.EntityFrameworkCore;
using StorageStrategy.Data.Context;
using StorageStrategy.Domain.Repository;
using StorageStrategy.Models;
using StorageStrategy.Utils.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageStrategy.Data.Repository
{
    public class ExpensesRepository : RepositoryBase<ExpensesEntity>, IExpensesRepository
    {
        public ExpensesRepository(StorageDbContext context) : base(context)
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

        public Task<ExpensesEntity> GetExpensesByIdAsync(int expensesId, int companyId)
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

        public async Task<List<ExpensesEntity>> ToList(int companyId)
        {
            return await _context.Expenses.Where(p => p.CompanyId == companyId).ToListAsync();
        }

        public void UpdateExpenseType(ExpensesTypeEntity expensesType)
        {
            _context.ExpensesType.Update(expensesType);
        }
    }
}
