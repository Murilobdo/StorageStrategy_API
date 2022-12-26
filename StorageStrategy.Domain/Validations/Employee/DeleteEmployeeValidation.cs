namespace StorageStrategy.Domain.Validations.Employee
{
    public class DeleteEmployeeValidation : EmployeeValidationBase
    {
        public DeleteEmployeeValidation()
        {
            ValidationId();
            ValidationCompanyId();
        }
    }
}
