namespace StorageStrategy.Domain.Validations.Employee
{
    public class UpdateEmployeeValidation : EmployeeValidationBase
    {
        public UpdateEmployeeValidation()
        {
            ValidationId();
            ValidationCompanyId();
            ValidationComission();
            ValidationName();
            ValidationJobRole();
            ValidationPassword();
            ValidationEmail();
        }
    }
}
