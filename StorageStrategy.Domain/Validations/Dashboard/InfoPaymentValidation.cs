using FluentValidation;
using StorageStrategy.Domain.Commands.Dashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageStrategy.Domain.Validations.Dashboard
{
    public class InfoPaymentValidation : AbstractValidator<InfoPaymentCommand>
    {
        public InfoPaymentValidation()
        {
            RuleFor(p => p.CompanyId)
                .GreaterThan(0)
                .WithMessage("O Id da Empresa e obrigatório");

            RuleFor(p => p.Month)
                .GreaterThan(0)
                .WithMessage("O Mês e obrigatório");
        }
    }
}
