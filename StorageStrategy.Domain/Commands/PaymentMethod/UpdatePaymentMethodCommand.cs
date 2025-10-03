using StorageStrategy.Domain.Validations.PaymentMethod;
using StorageStrategy.Models;

namespace StorageStrategy.Domain.Commands.PaymentMethod;

public record UpdatePaymentMethodCommand : PaymentCommandBase, IValidation
{
    public bool IsValid()
    {
        return new UpdatePaymentMethodValidation().Validate(this).IsValid;
    }

    public List<Error> GetErros()
    {
        return new UpdatePaymentMethodValidation().Validate(this)
            .Errors.Select(p => new Error(p.ErrorMessage)).ToList();
    }
}