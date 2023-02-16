using FluentValidation;
using StorageStrategy.Domain.Commands.Expenses;

namespace StorageStrategy.Domain.Validations.Expenses
{
    public class ExpensesTypeValidationBase : AbstractValidator<ExpensesTypeCommandBase>
    {
        protected void ValidationId() => RuleFor(p => p.ExpenseTypeId)
            .GreaterThan(0)
            .WithMessage("O Id e obrigatório");

        protected void ValidationCompanyId() => RuleFor(p => p.CompanyId)
           .GreaterThan(0)
           .WithMessage("O Id da empresa e obrigatório");

        protected void ValidationDescription() => RuleFor(p => p.Description)
            .NotEmpty()
            .WithMessage("A Descrição e obrigatória");
    }
}
