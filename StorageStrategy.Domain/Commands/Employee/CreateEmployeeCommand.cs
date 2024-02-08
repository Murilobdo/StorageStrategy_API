using StorageStrategy.Domain.Validations.Employee;
using StorageStrategy.Models;

namespace StorageStrategy.Domain.Commands.Employee
{
    public record class CreateEmployeeCommand : EmployeeCommandBase, IValidation
    {
       
        public CreateEmployeeCommand()
        {

        }

        public CreateEmployeeCommand(string name, int comission, EmployeeRole jobRole, 
            string email, string password, bool isActive, int companyId) 
        {
            Name = name;
            Comission = comission;
            JobRole = jobRole;
            Email = email;
            Password = password;
            IsActive = isActive;
            CompanyId = companyId;
        }

        public bool IsValid() => new CreateEmployeeValidation().Validate(this).IsValid;
        public List<Error> GetErros() => new CreateEmployeeValidation().Validate(this)
            .Errors.Select(p => new Error(p.ErrorMessage)).ToList();
    }
}
