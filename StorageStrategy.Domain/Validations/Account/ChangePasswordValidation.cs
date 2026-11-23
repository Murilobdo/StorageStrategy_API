using FluentValidation;
using StorageStrategy.Domain.Commands.Login;

namespace StorageStrategy.Domain.Validations.Account;

public class ChangePasswordValidation : AbstractValidator<ChangePasswordCommand>
{
    public const int MinimumPasswordLength = 8;

    public ChangePasswordValidation()
    {
        RuleFor(c => c.CurrentPassword)
            .NotEmpty()
            .WithMessage("A senha atual é obrigatória");

        RuleFor(c => c.NewPassword)
            .NotEmpty()
            .WithMessage("A nova senha é obrigatória")
            .MinimumLength(MinimumPasswordLength)
            .WithMessage($"A nova senha deve ter pelo menos {MinimumPasswordLength} caracteres")
            .NotEqual(c => c.CurrentPassword)
            .WithMessage("A nova senha deve ser diferente da atual");
    }
}
