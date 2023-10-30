using FluentValidation;
using StorageStrategy.Domain.Commands.Dashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageStrategy.Domain.Validations.Dashboard
{
    public class TotalCostPricePerDayValidation : AbstractValidator<TotalCostPricePerDayCommand>
    {
        public TotalCostPricePerDayValidation()
        {
            RuleFor(p => p.CompanyId)
                .GreaterThan(0)
                .WithMessage("O Id da Empresa e obrigatório");

            RuleFor(p => p.Month)
                .NotNull()
                .WithMessage("O Mês e obrigatório");

        }
    }
}
