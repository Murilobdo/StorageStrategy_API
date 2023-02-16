using StorageStrategy.Domain.Validations.Expenses;
using StorageStrategy.Domain.Validations.Product;
using StorageStrategy.Models;

namespace StorageStrategy.Domain.Commands.Expenses
{
    public record class CreateExpensesTypeCommand : ExpensesTypeCommandBase, IValidation
    {

        public CreateExpensesTypeCommand(int expenseTypeId, int companyId, string description)
        {
            ExpenseTypeId = expenseTypeId;
            CompanyId = companyId;
            Description = description;
        }

        public bool IsValid() => new CreateExpensesTypeValidation().Validate(this).IsValid;
        public List<Error> GetErros() => new CreateExpensesTypeValidation().Validate(this)
            .Errors.Select(p => new Error(p.PropertyName, p.ErrorMessage)).ToList();
    }
}
