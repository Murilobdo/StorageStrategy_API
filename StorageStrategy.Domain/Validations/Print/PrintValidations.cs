using FluentValidation;
using StorageStrategy.Domain.Commands.Print;
using StorageStrategy.Models;

namespace StorageStrategy.Domain.Validations.Print;

public class CreatePrintJobValidation : AbstractValidator<CreatePrintJobCommand>
{
    public CreatePrintJobValidation()
    {
        RuleFor(c => c.CommandId)
            .GreaterThan(0)
            .WithMessage("Informe a comanda");

        RuleFor(c => c.CompanyId)
            .GreaterThan(0)
            .WithMessage("O Id da empresa é obrigatório");
    }
}

public class SavePrintStationValidation : AbstractValidator<SavePrintStationCommand>
{
    public SavePrintStationValidation()
    {
        RuleFor(c => c.CompanyId)
            .GreaterThan(0)
            .WithMessage("O Id da empresa é obrigatório");

        RuleFor(c => c.Name)
            .Must(n => !string.IsNullOrWhiteSpace(n) && n.Trim().Length is >= 2 and <= 60)
            .WithMessage("O nome da impressora deve ter entre 2 e 60 caracteres");

        RuleFor(c => c.PaperWidthMm)
            .Equal(PrintStationEntity.DefaultPaperWidthMm)
            .WithMessage("Só o papel de 58mm é suportado");

        RuleFor(c => c.Encoding)
            .IsInEnum()
            .WithMessage("Codificação inválida");
    }
}
