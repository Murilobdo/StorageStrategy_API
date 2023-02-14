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

            RuleFor(p => p.Date)
                .NotNull()
                .WithMessage("A Data e obrigatória");
        }
    }
}
