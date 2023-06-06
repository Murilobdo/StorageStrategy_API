using StorageStrategy.Models;

namespace StorageStrategy.Domain.Commands.Employee
{
    public record class EmployeeCommandBase : CommandBase
    {
        

        public EmployeeCommandBase()
        {

        }

        public EmployeeCommandBase(int employeeId, string name, int comission, string jobRole, string email, string password, bool isActive, int companyId)
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

        public int EmployeeId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Comission { get; set; }
        public string JobRole { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public int CompanyId { get; set; }
    }
}
