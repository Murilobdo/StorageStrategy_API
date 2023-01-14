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

        protected void ValidationEmployeeId() => RuleFor(p => p.EmployeeId)
            .GreaterThan(0)
            .WithMessage("O funcionário e obrigatório");

        protected void ValidationPayament() => RuleFor(p => p.Payment)
           .IsInEnum()
           .WithMessage("O tipo do pagamento e obrigatório");

        protected void ValidationProducts() => RuleFor(p => p.Products.Count)
            .GreaterThan(0)
            .WithMessage("A comanda deve conter produtos nela.");
    }
}
