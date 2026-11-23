namespace StorageStrategy.Models
{
    public class CompanyEntity
    {
        public int CompanyId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime Validate { get; set; }
        public string CNPJ { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public List<ProductEntity> Products { get; set; } = new();
        public List<CategoryEntity> Categorys { get; set; } = new();
        public List<EmployeeEntity> Employees { get; set; } = new();
        public List<CommandEntity> Commands { get; set; } = new();
        public List<ExpenseEntity> Expenses { get; set; } = new();
        public List<StockHistoryEntity> StockHistory { get; set; } = new();

        public CompanyEntity InitAdmin(int companyId, string name, string description, DateTime createAt, DateTime validate)
        {
            CompanyId = companyId;
            Name = name;
            Description = description;
            IsActive = true;
            CreateAt = createAt;
            Validate = validate;

            return this;
        }

        public CompanyEntity()
        {
            IsActive = true;
        }
    }
}
