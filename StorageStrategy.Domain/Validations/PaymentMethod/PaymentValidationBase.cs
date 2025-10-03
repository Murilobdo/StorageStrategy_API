using FluentValidation;
using StorageStrategy.Domain.Commands.PaymentMethod;

namespace StorageStrategy.Domain.Validations.PaymentMethod;

public class PaymentValidationBase : AbstractValidator<PaymentCommandBase>
{
    public void ValidationPaymentMethodId()
    {
        RuleFor(c => c.PaymentMethodId)
            .GreaterThan(0)
            .WithMessage("O Id do método de pagamento deve ser maior que 0");
    }

    public void ValidationCompanyId()
    {
        RuleFor(c => c.CompanyId)
            .GreaterThan(0)
            .WithMessage("O Id e obrigatorio");
    }

    public void ValidationCompany()
    {
        RuleFor(c => c.Company)
            .NotEmpty()
            .WithMessage("A Empresa de obrigatorio");
    }

    public void ValidationDebitFee()
    {
        RuleFor(c => c.DebitFee)
            .GreaterThanOrEqualTo(0)
            .WithMessage("A taxa do débito deve ser maior ou igual a 0%");
    }

    public void ValidationCreditFee()
    {
        RuleFor(c => c.CreditFee)
            .GreaterThanOrEqualTo(0)
            .WithMessage("A taxa do crédito deve ser maior ou igual a 0%");
    }
    
}