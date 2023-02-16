namespace StorageStrategy.Models
{
    public class CompanyEntity
    {
        public int CompanyId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime CreateAt { get; set; }
        public List<ProductEntity> Products { get; set; } = new();
        public List<CategoryEntity> Categorys { get; set; } = new();
        public List<EmployeeEntity> Employees { get; set; } = new();
        public List<CommandEntity> Commands { get; set; } = new();
        public List<ExpensesEntity> Expenses { get; set; } = new();

        public CompanyEntity(int companyId, string name, string description, DateTime createAt)
        {
            CompanyId = companyId;
            Name = name;
            Description = description;
            IsActive = true;
            CreateAt = createAt;
        }

        public CompanyEntity()
        {
            IsActive = true;
        }
    }
}
