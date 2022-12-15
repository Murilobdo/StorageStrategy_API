using FluentValidation;
using StorageStrategy.Domain.Commands.Category;

namespace StorageStrategy.Domain.Validations.Category
{
    public class CategoryBaseValidation : AbstractValidator<CategoryCommandBase>
    {
        protected void validationId() => RuleFor(p => p.CategoryId)
            .GreaterThan(0)
            .WithMessage("O Id e obrigatório");

        protected void validationCompanyId() => RuleFor(p => p.CompanyId)
           .GreaterThan(0)
           .WithMessage("O Id da empresa e obrigatório");

        protected void validationName() => RuleFor(p => p.Name)
            .NotEmpty()
            .WithMessage("O Nome e obrigatório");
    }
}
