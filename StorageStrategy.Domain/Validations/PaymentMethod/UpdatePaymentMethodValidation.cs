namespace StorageStrategy.Domain.Validations.PaymentMethod;

public class UpdatePaymentMethodValidation : PaymentValidationBase
{
    public UpdatePaymentMethodValidation()
    {
        ValidationPaymentMethodId();
        ValidationCompanyId();
        ValidationCompany();
        ValidationDebitFee();
        ValidationCreditFee();
    }
    
}