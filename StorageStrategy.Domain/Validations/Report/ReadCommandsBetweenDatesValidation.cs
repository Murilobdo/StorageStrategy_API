using System.Data;
using FluentValidation;
using StorageStrategy.Domain.Commands.Report;

namespace StorageStrategy.Domain.Validations.Report
{
    public class ReadCommandsBetweenDatesValidation : AbstractValidator<ReadCommandsBetweenDatesCommand>
    {
        public ReadCommandsBetweenDatesValidation()
        {
            RuleFor(p => p.CompanyId)
                .GreaterThan(0)
                .WithMessage("O Id da empresa e obrigatório");

            RuleFor(p => p.InitialDate)
                .NotEmpty()
                .WithMessage("A data inicial e obrigatória");
            
            RuleFor(p => p.FinalDate)
                .NotEmpty()
                .WithMessage("A data final e obrigatória");
        }
    }
}