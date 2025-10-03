namespace StorageStrategy.Domain.Validations.PaymentMethod;

public class CreatePaymentMethodValidation : PaymentValidationBase
{
    public CreatePaymentMethodValidation()
    {
        ValidationCompanyId();
        ValidationCompany();
        ValidationDebitFee();
        ValidationCreditFee();
    }
}