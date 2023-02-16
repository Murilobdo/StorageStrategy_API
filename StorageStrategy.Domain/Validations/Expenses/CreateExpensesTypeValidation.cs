namespace StorageStrategy.Domain.Validations.Expenses
{
    public class CreateExpensesTypeValidation : ExpensesTypeValidationBase
    {
        public CreateExpensesTypeValidation()
        {
            ValidationCompanyId();
            ValidationDescription();
        }
    }
}
