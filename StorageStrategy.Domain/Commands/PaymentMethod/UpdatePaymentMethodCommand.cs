using StorageStrategy.Domain.Validations.PaymentMethod;
using StorageStrategy.Models;

namespace StorageStrategy.Domain.Commands.PaymentMethod;

public record CreatePaymentMethodCommand : PaymentCommandBase, IValidation
{
    public bool IsValid()
    {
        return new CreatePaymentMethodValidation().Validate(this).IsValid;
    }

    public List<Error> GetErros()
    {
        return new CreatePaymentMethodValidation().Validate(this)
            .Errors.Select(p => new Error(p.ErrorMessage)).ToList();
    }
}