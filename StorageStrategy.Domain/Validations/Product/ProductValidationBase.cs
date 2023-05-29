using FluentValidation;
using StorageStrategy.Domain.Commands.Products;

namespace StorageStrategy.Domain.Validations.Product
{
    public class ProductValidationBase : AbstractValidator<ProductCommandBase>
    {
        protected void ValidationId() => RuleFor(p => p.ProductId)
            .GreaterThan(0)
            .WithMessage("O Id e obrigatório");

        protected void ValidationCompanyId() => RuleFor(p => p.CompanyId)
           .GreaterThan(0)
           .WithMessage("O Id da empresa e obrigatório");

        protected void ValidationName() => RuleFor(p => p.Name)
            .NotEmpty()
            .WithMessage("O Nome e obrigatório");

        protected void ValidationCost() => RuleFor(p => p.Cost)
            .GreaterThan(0)
            .WithMessage("O valor de custo deve ser maior do que 0");

        protected void ValidationPrice() => RuleFor(p => p.Price)
            .GreaterThan(0)
            .WithMessage("O valor de venda deve ser maior do que 0");

        protected void ValidationCategoryId() => RuleFor(p => p.CategoryId)
            .GreaterThan(0)
            .WithMessage("O id da categoria e obrigatório");

        protected void ValidationQtd() => RuleFor(p => p.Qtd)
            .GreaterThan(0)
            .WithMessage("A quantidade deve ser maior do que 0");
    }
}
