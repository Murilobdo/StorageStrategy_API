using StorageStrategy.Models;

namespace StorageStrategy.Domain.Commands.Employee
{
    public record class EmployeeCommandBase : CommandBase
    {
        public EmployeeCommandBase(int employeeId, string name, int comission, string jobRole, bool isActive, int companyId)
        {
            EmployeeId = employeeId;
            Name = name;
            Comission = comission;
            JobRole = jobRole;
            IsActive = isActive;
            CompanyId = companyId;
        }

        public EmployeeCommandBase()
        {

        }

        public int EmployeeId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Comission { get; set; }
        public string JobRole { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public int CompanyId { get; set; }
    }
}
