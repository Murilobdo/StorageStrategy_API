using FluentValidation;
using StorageStrategy.Domain.Commands.PixCredential;

namespace StorageStrategy.Domain.Validations.PixCredential;

public class SavePixCredentialValidation : AbstractValidator<SavePixCredentialCommand>
{
    public SavePixCredentialValidation()
    {
        RuleFor(c => c.CompanyId)
            .GreaterThan(0)
            .WithMessage("O Id da empresa é obrigatório");

        RuleFor(c => c.AccessToken)
            .NotEmpty()
            .WithMessage("O Access Token do Mercado Pago é obrigatório")
            .Must(token => token.StartsWith("APP_USR-", StringComparison.OrdinalIgnoreCase) || token.StartsWith("TEST-", StringComparison.OrdinalIgnoreCase))
            .WithMessage("O Access Token informado não parece um token válido do Mercado Pago");

        RuleFor(c => c.WebhookSecret)
            .NotEmpty()
            .WithMessage("O segredo do webhook é obrigatório");
    }
}
