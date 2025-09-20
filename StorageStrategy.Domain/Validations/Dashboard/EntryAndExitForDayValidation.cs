using FluentValidation;
using StorageStrategy.Domain.Commands.Dashboard;

namespace StorageStrategy.Domain.Validations.Dashboard
{
    public class EntryAndExitForDayValidation : AbstractValidator<EntryAndExitForDayCommand>
    {
        public EntryAndExitForDayValidation()
        {
            RuleFor(p => p.CompanyId)
                .GreaterThan(0)
                .WithMessage("O Id da Empresa e obrigatório");

            RuleFor(p => p.Month)
                .GreaterThan(0)
                .WithMessage("O Mês e obrigatório");
            
            RuleFor(p => p.Year)
                .GreaterThan(0)
                .WithMessage("O Ano e obrigatório");
        }
    }
}
