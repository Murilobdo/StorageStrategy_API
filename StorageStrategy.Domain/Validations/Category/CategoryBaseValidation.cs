using FluentValidation;
using StorageStrategy.Domain.Commands.Category;

namespace StorageStrategy.Domain.Validations.Category
{
    public class CategoryBaseValidation : AbstractValidator<CategoryCommandBase>
    {
        protected void validationId() => RuleFor(p => p.CategoryId)
            .GreaterThan(0)
            .WithMessage("Campo obrigatório");

        protected void validationCompanyId() => RuleFor(p => p.CompanyId)
           .GreaterThan(0)
           .WithMessage("Campo obrigatório");

        protected void validationName() => RuleFor(p => p.Name)
            .NotEmpty()
            .WithMessage("Campo obrigatório");
    }
}
