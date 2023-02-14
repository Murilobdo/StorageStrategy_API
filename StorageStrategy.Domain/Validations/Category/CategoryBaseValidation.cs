using FluentValidation;
using StorageStrategy.Domain.Commands.Category;

namespace StorageStrategy.Domain.Validations.Category
{
    public class CategoryBaseValidation : AbstractValidator<CategoryCommandBase>
    {
        protected void ValidationId() => RuleFor(p => p.CategoryId)
            .GreaterThan(0)
            .WithMessage("O Id e obrigatório");

        protected void ValidationCompanyId() => RuleFor(p => p.CompanyId)
           .GreaterThan(0)
           .WithMessage("O Id da Empresa e obrigatório");

        protected void ValidationName() => RuleFor(p => p.Name)
            .NotEmpty()
            .WithMessage("O Nome e obrigatório");
    }
}
