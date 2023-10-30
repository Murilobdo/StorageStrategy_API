using StorageStrategy.Domain.Validations.Expenses;
using StorageStrategy.Domain.Validations.Product;
using StorageStrategy.Models;

namespace StorageStrategy.Domain.Commands.Expenses
{
    public record class DeleteExpenseCommand : ExpenseCommandBase, IValidation
    {
        public DeleteExpenseCommand()
        {

        }

        public DeleteExpenseCommand(int expenseId, int companyId)
        {
            ExpenseId = expenseId;
            CompanyId = companyId;
        }

        public bool IsValid() => new DeleteExpensesValidation().Validate(this).IsValid;
        public List<Error> GetErros() => new DeleteExpensesValidation().Validate(this)
            .Errors.Select(p => new Error(p.ErrorMessage)).ToList();
    }
}
