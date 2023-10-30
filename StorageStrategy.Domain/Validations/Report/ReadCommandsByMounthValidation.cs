using FluentValidation;
using StorageStrategy.Domain.Commands.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageStrategy.Domain.Validations.Report
{
    public class ReadCommandsByMounthValidation : AbstractValidator<FilterDashboard>
    {
        public ReadCommandsByMounthValidation()
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
