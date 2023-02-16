namespace StorageStrategy.Domain.Validations.Expenses
{
    public class DeleteExpensesValidation : ExpensesValidationBase
    {
        public DeleteExpensesValidation()
        {
            ValidationId();
            ValidationCompanyId();
        }
    }
}
