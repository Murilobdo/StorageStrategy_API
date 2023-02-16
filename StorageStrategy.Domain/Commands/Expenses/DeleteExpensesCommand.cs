using StorageStrategy.Domain.Validations.Expenses;
using StorageStrategy.Domain.Validations.Product;
using StorageStrategy.Models;

namespace StorageStrategy.Domain.Commands.Expenses
{
    public record class DeleteExpensesCommand : ExpensesCommandBase, IValidation
    {
        public DeleteExpensesCommand()
        {

        }

        public DeleteExpensesCommand(int expenseId, int companyId)
        {
            ExpenseId = expenseId;
            CompanyId = companyId;
        }

        public bool IsValid() => new DeleteExpensesValidation().Validate(this).IsValid;
        public List<Error> GetErros() => new DeleteExpensesValidation().Validate(this)
            .Errors.Select(p => new Error(p.PropertyName, p.ErrorMessage)).ToList();
    }
}
