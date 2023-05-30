using StorageStrategy.Domain.Validations.Employee;
using StorageStrategy.Domain.Validations.Product;
using StorageStrategy.Models;

namespace StorageStrategy.Domain.Commands.Employee
{
    public record class CreateEmployeeCommand : EmployeeCommandBase, IValidation
    {
        public CreateEmployeeCommand(int employeeId, string name, int comission, string jobRole, bool isActive, int companyId, string password)
        {
            EmployeeId = employeeId;
            Name = name;
            Comission = comission;
            JobRole = jobRole;
            IsActive = isActive;
            CompanyId = companyId;
            Password = password;
        }
        public CreateEmployeeCommand()
        {

        }

        public bool IsValid() => new CreateEmployeeValidation().Validate(this).IsValid;
        public List<Error> GetErros() => new CreateEmployeeValidation().Validate(this)
            .Errors.Select(p => new Error(p.PropertyName, p.ErrorMessage)).ToList();
    }
}
