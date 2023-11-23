using StorageStrategy.Domain.Validations.Employee;
using StorageStrategy.Domain.Validations.Product;
using StorageStrategy.Models;

namespace StorageStrategy.Domain.Commands.Employee
{
    public record class UpdateEmployeeCommand : EmployeeCommandBase, IValidation
    {
      
        public UpdateEmployeeCommand()
        {

        }

        public UpdateEmployeeCommand(int employeeId, string name, int comission, EmployeeRole jobRole, 
            string email, string password, bool isActive, int companyId) : 
            base(employeeId, name, comission, jobRole, email, password, isActive, companyId)
        {
            EmployeeId = employeeId;
            Name = name;
            Comission = comission;
            JobRole = jobRole;
            Email = email;
            Password = password;
            IsActive = isActive;
            CompanyId = companyId;
        }

        public bool IsValid() => new UpdateEmployeeValidation().Validate(this).IsValid;
        public List<Error> GetErros() => new UpdateEmployeeValidation().Validate(this)
            .Errors.Select(p => new Error(p.ErrorMessage)).ToList();
    }
}
