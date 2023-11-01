namespace StorageStrategy.Models
{
    public class EmployeeEntity 
    {
        public int EmployeeId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int Comission { get; set; }
        public string JobRole { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public string PasswordHash { get; set; } = string.Empty;
        public int CompanyId { get; set; }
        public virtual CompanyEntity Company { get; set; }

        public EmployeeEntity(
            int employeeId, 
            string name, 
            string email, 
            int comission, 
            string jobRole, 
            bool isActive, 
            string passwordHash, 
            int companyId
        ) {
            EmployeeId = employeeId;
            Name = name;
            Email = email;
            Comission = comission;
            JobRole = jobRole;
            IsActive = isActive;
            PasswordHash = passwordHash;
            CompanyId = companyId;
        }

        public EmployeeEntity()
        {
            
        }
    }
}
