using FluentValidation;
using StorageStrategy.Domain.Commands.PixCharge;

namespace StorageStrategy.Domain.Validations.PixCharge;

public class CreatePixChargeValidation : AbstractValidator<CreatePixChargeCommand>
{
    public CreatePixChargeValidation()
    {
        RuleFor(c => c.CommandId)
            .GreaterThan(0)
            .WithMessage("A comanda é obrigatória");

        RuleFor(c => c.CompanyId)
            .GreaterThan(0)
            .WithMessage("O Id da empresa é obrigatório");
    }
}
