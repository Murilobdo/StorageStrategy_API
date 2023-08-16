using StorageStrategy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageStrategy.Domain.Repository
{
    public interface IExpenseRepository : IRepositoryBase<ExpenseEntity>
    {
        Task AddExpenseTypeAsync(ExpensesTypeEntity expensesType);
        void UpdateExpenseType(ExpensesTypeEntity expensesType);
        void DeleteExpenseType(ExpensesTypeEntity expensesTypeId);
        Task<List<ExpensesTypeEntity>> ReadExpensesTypeAsync(int companyId);
        Task<ExpenseEntity> GetExpensesByIdAsync(int expensesId, int companyId);
        Task<ExpensesTypeEntity> GetExpensesTypeByIdAsync(int expensesTypeId, int companyId);
        Task<ExpensesTypeEntity> GetExpensesTypeByDescriptionAsync(string description, int companyId);
        Task<List<ExpenseEntity>> ToList(int companyId);
    }
}
