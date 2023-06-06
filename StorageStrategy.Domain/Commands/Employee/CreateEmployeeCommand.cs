using StorageStrategy.Domain.Validations.Employee;
using StorageStrategy.Models;

namespace StorageStrategy.Domain.Commands.Employee
{
    public record class CreateEmployeeCommand : EmployeeCommandBase, IValidation
    {
       
        public CreateEmployeeCommand()
        {

        }

        public CreateEmployeeCommand(int employeeId, string name, int comission, string jobRole, 
            string email, string password, bool isActive, int companyId) : 
            base(employeeId, name, comission, jobRole, email, password, isActive, companyId)
        {
        }

        public bool IsValid() => new CreateEmployeeValidation().Validate(this).IsValid;
        public List<Error> GetErros() => new CreateEmployeeValidation().Validate(this)
            .Errors.Select(p => new Error(p.PropertyName, p.ErrorMessage)).ToList();
    }
}
