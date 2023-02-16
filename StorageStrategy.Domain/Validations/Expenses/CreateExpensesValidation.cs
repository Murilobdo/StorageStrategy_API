namespace StorageStrategy.Domain.Validations.Expenses
{
    public class CreateExpensesValidation : ExpensesValidationBase
    {
        public CreateExpensesValidation()
        {
            ValidationCompanyId();
            ValidationDescription();
            ValidationDate();
            ValidationExpensesTypeId();
        }
    }
}
