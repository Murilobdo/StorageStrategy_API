using StorageStrategy.Domain.Validations.Employee;
using StorageStrategy.Domain.Validations.Product;
using StorageStrategy.Models;

namespace StorageStrategy.Domain.Commands.Employee
{
    public record class DeleteEmployeeCommand : EmployeeCommandBase, IValidation
    {
        public DeleteEmployeeCommand(int employeeId, int companyId)
        {
            EmployeeId = employeeId;
            CompanyId = companyId;
        }

        public DeleteEmployeeCommand()
        {

        }

        public bool IsValid() => new DeleteEmployeeValidation().Validate(this).IsValid;
        public List<Error> GetErros() => new DeleteEmployeeValidation().Validate(this)
            .Errors.Select(p => new Error(p.PropertyName, p.ErrorMessage)).ToList();
    }
}
