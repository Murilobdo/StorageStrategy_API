using FluentValidation;
using StorageStrategy.Domain.Commands.Dashboard;

namespace StorageStrategy.Domain.Validations.Dashboard
{
    public class SalesPerEmployeeValidation : AbstractValidator<SalesPerEmployeeCommand>
    {
        public SalesPerEmployeeValidation()
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
