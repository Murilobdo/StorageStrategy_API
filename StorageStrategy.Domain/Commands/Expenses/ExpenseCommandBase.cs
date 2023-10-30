using StorageStrategy.Models;

namespace StorageStrategy.Domain.Commands.Expenses
{
    public record class ExpenseCommandBase : CommandBase
    {
        public ExpenseCommandBase()
        {

        }

        public ExpenseCommandBase(int expenseId, int companyId, string description, int expensesTypeId, DateTime createAt)
        {
            ExpenseId = expenseId;
            CompanyId = companyId;
            Description = description;
            ExpensesTypeId = expensesTypeId;
            CreateAt = createAt;
        }

        public int ExpenseId { get; set; }
        public int CompanyId { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal ExpenseValue { get; set; }
        public int ExpensesTypeId { get; set; }
        public DateTime CreateAt { get; set; }
    }
}
