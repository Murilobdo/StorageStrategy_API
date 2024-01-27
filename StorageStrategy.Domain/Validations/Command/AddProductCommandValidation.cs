using FluentValidation;
using StorageStrategy.Domain.Commands.Command;

namespace StorageStrategy.Domain.Validations.Command
{
    public class AddProductCommandValidation : AbstractValidator<AddProductCommandCommand>
    {
        public AddProductCommandValidation()
        {
            ValidateCommandId();
            ValidateCompanyId();
            ValidateItems();
        }

        protected void ValidateCommandId() => RuleFor(p => p.CommandId)
            .GreaterThan(0)
            .WithMessage("O id da comanda e obrigatorio");

        protected void ValidateCompanyId() => RuleFor(p => p.CompanyId)
            .GreaterThan(0)
            .WithMessage("O id da empresa e obrigatorio");

        protected void ValidateItems() => RuleFor(p => p.Items.Count)
            .GreaterThanOrEqualTo(1)
            .WithMessage("A comanda deve possuir itens");
    }
}
