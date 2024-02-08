using StorageStrategy.Domain.Validations.Dashboard;
using StorageStrategy.Models;

namespace StorageStrategy.Domain.Commands.Dashboard
{
    public record class SalesPerEmployeeCommand : CommandBase
    {
        public SalesPerEmployeeCommand()
        {
                
        }

        public SalesPerEmployeeCommand(int companyId, int month)
        {
            CompanyId = companyId;
            Month = month;
        }

        public int CompanyId { get; set; }
        public int Month { get; set; }

        public List<Error> GetErros() => new SalesPerEmployeeValidation().Validate(this)
           .Errors.Select(p => new Error(p.ErrorMessage)).ToList();

        public bool IsValid() => new SalesPerEmployeeValidation().Validate(this).IsValid;
    }
}
