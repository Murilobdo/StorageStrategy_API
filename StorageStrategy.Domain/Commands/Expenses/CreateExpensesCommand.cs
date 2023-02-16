using StorageStrategy.Domain.Validations.Expenses;
using StorageStrategy.Domain.Validations.Product;
using StorageStrategy.Models;

namespace StorageStrategy.Domain.Commands.Expenses
{
    public record class CreateExpensesCommand : ExpensesCommandBase, IValidation
    {
        public CreateExpensesCommand()
        {

        }

        public CreateExpensesCommand(int companyId, string description, int expensesTypeId, DateTime createAt)
        {
            CompanyId = companyId;
            Description = description;
            ExpensesTypeId = expensesTypeId;
            CreateAt = createAt;
        }

        public bool IsValid() => new CreateExpensesValidation().Validate(this).IsValid;
        public List<Error> GetErros() => new CreateExpensesValidation().Validate(this)
            .Errors.Select(p => new Error(p.PropertyName, p.ErrorMessage)).ToList();
    }
}
