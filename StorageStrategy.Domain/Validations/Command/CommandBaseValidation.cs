using FluentValidation;
using StorageStrategy.Domain.Commands.Command;
using StorageStrategy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageStrategy.Domain.Validations.Command
{
    public class CommandBaseValidation : AbstractValidator<CommandCommandBase>
    {
        protected void ValidationId() => RuleFor(p => p.CommandId)
            .GreaterThan(0)
            .WithMessage("O Id e obrigatório");

        protected void ValidationName() => RuleFor(p => p.Name)
            .NotEmpty()
            .WithMessage("O Nome e obrigatório");

        protected void ValidationCompanyId() => RuleFor(p => p.CompanyId)
            .GreaterThan(0)
            .WithMessage("O Id da empresa e obrigatório");

        protected void ValidateDiscount() => RuleFor(p => p.Discount)
            .GreaterThanOrEqualTo(-1)
            .WithMessage("O desconto não pode ser negativo");

        protected void ValidateIncrease() => RuleFor(p => p.Increase)
            .GreaterThanOrEqualTo(-1)
            .WithMessage("O acréscimo não pode ser negativo");

        protected void ValidationEmployeeId() => RuleFor(p => p.EmployeeId)
            .GreaterThan(0)
            .WithMessage("O funcionário e obrigatório");

        protected void ValidationPayament() => RuleFor(p => p.Payment)
           .IsInEnum()
           .WithMessage("O tipo do pagamento e obrigatório");

        protected void ValidationProducts() => RuleFor(p => p.Items.Count)
            .GreaterThan(0)
            .WithMessage("Essa comanda não possui produtos");
    }
}
