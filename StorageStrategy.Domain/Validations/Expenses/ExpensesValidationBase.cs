using FluentValidation;
using StorageStrategy.Domain.Commands.Expenses;

namespace StorageStrategy.Domain.Validations.Expenses
{
    public class ExpensesValidationBase : AbstractValidator<ExpenseCommandBase>
    {
        protected void ValidationId() => RuleFor(p => p.ExpenseId)
            .GreaterThan(0)
            .WithMessage("O Id e obrigatório");

        protected void ValidationCompanyId() => RuleFor(p => p.CompanyId)
           .GreaterThan(0)
           .WithMessage("O Id da empresa e obrigatório");

        protected void ValidationExpensesTypeId() => RuleFor(p => p.ExpensesTypeId)
           .GreaterThan(0)
           .WithMessage("A caregoria da despesa e obrigatória");

        protected void ValidationDate() => RuleFor(p => p.CreateAt)
            .NotNull()
            .WithMessage("A data e obrigatória");

        protected void ValidationDescription() => RuleFor(p => p.Description)
            .NotEmpty()
            .WithMessage("A Descrição e obrigatória");

        protected void ValidationExpenseValue() => RuleFor(p => p.ExpenseValue)
           .GreaterThan(0)
           .WithMessage("O Valor da despesa deve ser maior do que 0");
    }
}
