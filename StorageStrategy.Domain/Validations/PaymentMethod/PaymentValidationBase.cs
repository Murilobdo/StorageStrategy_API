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

  
    public void ValidationTotalFee()
    {
        RuleFor(c => c.TotalFee)
            .GreaterThanOrEqualTo(0)
            .WithMessage("A taxa de venda deve ser maior ou igual a 0%");
    }
    
}