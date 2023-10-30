using StorageStrategy.Domain.Validations.Expenses;
using StorageStrategy.Domain.Validations.Product;
using StorageStrategy.Models;

namespace StorageStrategy.Domain.Commands.Expenses
{
    public record class CreateExpenseCommand : ExpenseCommandBase, IValidation
    {
        public CreateExpenseCommand()
        {

        }

        public CreateExpenseCommand(int companyId, string description, int expensesTypeId, DateTime createAt, decimal expenseValue)
        {
            CompanyId = companyId;
            Description = description;
            ExpensesTypeId = expensesTypeId;
            CreateAt = createAt;
            ExpenseValue = expenseValue;
        }

        public bool IsValid() => new CreateExpensesValidation().Validate(this).IsValid;
        public List<Error> GetErros() => new CreateExpensesValidation().Validate(this)
            .Errors.Select(p => new Error(p.PropertyName, p.ErrorMessage)).ToList();
    }
}
