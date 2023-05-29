using System.Data;
using FluentValidation;
using StorageStrategy.Domain.Commands.Report;

namespace StorageStrategy.Domain.Validations.Report
{
    public class ReadCommandsByMounthValidation : AbstractValidator<ReadCommandsByMounthCommand>
    {
        public ReadCommandsByMounthValidation()
        {
            RuleFor(p => p.CompanyId)
                .GreaterThan(0)
                .WithMessage("O Id da empresa e obrigatório");

        }
    }
}