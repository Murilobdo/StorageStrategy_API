using StorageStrategy.Domain.Validations.Expenses;
using StorageStrategy.Domain.Validations.Product;
using StorageStrategy.Models;

namespace StorageStrategy.Domain.Commands.Expenses
{
    public record class DeleteExpenseTypeCommand : ExpensesTypeCommandBase, IValidation
    {

        public DeleteExpenseTypeCommand(int expenseTypeId, int companyId)
        {
            ExpenseTypeId = expenseTypeId;
            CompanyId = companyId;
        }

        public bool IsValid() => new DeleteExpensesTypeValidation().Validate(this).IsValid;
        public List<Error> GetErros() => new DeleteExpensesTypeValidation().Validate(this)
            .Errors.Select(p => new Error(p.PropertyName, p.ErrorMessage)).ToList();
    }
}
