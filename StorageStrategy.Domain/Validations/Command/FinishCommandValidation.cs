using FluentValidation;
using StorageStrategy.Domain.Commands.Command;

namespace StorageStrategy.Domain.Validations.Command
{
    public class FinishCommandValidation : AbstractValidator<FinishCommandCommand>
    {
        public FinishCommandValidation()
        {
            ValidateCommandId();
            ValidatePayment();
            ValidateCompanyId();
        }

        protected void ValidateCommandId() => RuleFor(p => p.CommandId)
            .GreaterThan(0)
            .WithMessage("O id da comanda e obrigatório");

        protected void ValidateCompanyId() => RuleFor(p => p.CompanyId)
            .GreaterThan(0)
            .WithMessage("O id da empresa e obrigatório");

        protected void ValidatePayment() => RuleFor(p => p.Payment)
            .IsInEnum()
            .WithMessage("O tipo de pagamento e obrigatório");
    }
}
