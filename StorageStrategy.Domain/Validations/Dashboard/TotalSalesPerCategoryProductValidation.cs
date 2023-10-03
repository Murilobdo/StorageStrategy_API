using FluentValidation;
using StorageStrategy.Domain.Commands.Dashboard;

namespace StorageStrategy.Domain.Validations.Dashboard
{
    public class TotalSalesPerCategoryProductValidation : AbstractValidator<TotalSalesPerCategoryProductCommand>
    {
        public TotalSalesPerCategoryProductValidation()
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
