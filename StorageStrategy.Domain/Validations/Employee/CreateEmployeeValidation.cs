namespace StorageStrategy.Domain.Validations.Employee
{
    public class CreateEmployeeValidation : EmployeeValidationBase
    {
        public CreateEmployeeValidation()
        {
            ValidationCompanyId();
            ValidationComission();
            ValidationName();
            ValidationJobRole();
        }
    }
}
