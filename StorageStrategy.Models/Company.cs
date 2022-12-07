namespace StorageStrategy.Models
{
    public class Company
    {
        public int CompanyId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime CreateAt { get; set; }
        public List<Product> Products { get; set; } = new();
        public List<Category> Categorys { get; set; } = new();

        public Company(int companyId, string name, string description, DateTime createAt)
        {
            CompanyId = companyId;
            Name = name;
            Description = description;
            IsActive = true;
            CreateAt = createAt;
        }

        public Company()
        {
            IsActive = true;
        }
    }
}
