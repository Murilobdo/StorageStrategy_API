using StorageStrategy.Domain.Validations.Dashboard;
using StorageStrategy.Models;

namespace StorageStrategy.Domain.Commands.Dashboard
{
    public record class SalesPerEmployeeCommand : DashboardCommandBase
    {
        public SalesPerEmployeeCommand()
        {
                
        }

        public SalesPerEmployeeCommand(int companyId, int month, int year)
        {
            CompanyId = companyId;
            Month = month;
            Year = year;
        }

        public List<Error> GetErros() => new SalesPerEmployeeValidation().Validate(this)
           .Errors.Select(p => new Error(p.ErrorMessage)).ToList();

        public bool IsValid() => new SalesPerEmployeeValidation().Validate(this).IsValid;
    }
}
