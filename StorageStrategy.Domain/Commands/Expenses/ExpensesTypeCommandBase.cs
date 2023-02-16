using StorageStrategy.Models;

namespace StorageStrategy.Domain.Commands.Expenses
{
    public record class ExpensesTypeCommandBase : CommandBase
    {
        public ExpensesTypeCommandBase(int expenseTypeId, int companyId, string description)
        {
            ExpenseTypeId = expenseTypeId;
            CompanyId = companyId;
            Description = description;
        }

        public ExpensesTypeCommandBase()
        {

        }

        public int ExpenseTypeId { get; set; }
        public int CompanyId { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}
