namespace StorageStrategy.Domain.Validations.Expenses
{
    public class DeleteExpensesTypeValidation : ExpensesTypeValidationBase
    {
        public DeleteExpensesTypeValidation()
        {
            ValidationId();
            ValidationCompanyId();
        }
    }
}
