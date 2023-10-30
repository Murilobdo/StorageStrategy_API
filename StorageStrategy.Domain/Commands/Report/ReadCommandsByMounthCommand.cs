using StorageStrategy.Domain.Validations.Report;
using StorageStrategy.Models;

namespace StorageStrategy.Domain.Commands.Report
{
    public record class ReadCommandsByMounthCommand : FilterDashboard
    {
        public ReadCommandsByMounthCommand(int companyId, int month, int? employeeId)
        {
            CompanyId = companyId;
            Month = month;
            EmployeeId = employeeId;
        }
    }
}
