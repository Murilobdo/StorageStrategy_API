using FluentValidation;
using StorageStrategy.Domain.Commands.Dashboard;

namespace StorageStrategy.Domain.Validations.Dashboard
{
    public class EntryAndExitOfMonthValidation : AbstractValidator<EntryAndExitOfMonthCommand>
    {
        public EntryAndExitOfMonthValidation()
        {
            RuleFor(p => p.CompanyId)
                .GreaterThan(0)
                .WithMessage("O Id da Empresa e obrigatório");

            RuleFor(p => p.CurrentDate)
                .NotNull()
                .WithMessage("A Data e obrigatória");

        }
    }
}
