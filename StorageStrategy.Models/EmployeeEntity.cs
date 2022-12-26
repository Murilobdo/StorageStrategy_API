namespace StorageStrategy.Models
{
    public class EmployeeEntity 
    {
        public int EmployeeId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Comission { get; set; }
        public string JobRole { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public int CompanyId { get; set; }
        public virtual CompanyEntity Company { get; set; }
    }
}
