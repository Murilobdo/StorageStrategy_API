
namespace StorageStrategy.Models
{
    public class CategoryEntity
    {
        public int CategoryId { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public List<ProductEntity> Products { get; set; } = new();
        public virtual CompanyEntity Company { get; set; }
        public int CompanyId { get; set; }

        public CategoryEntity()
        {

        }

        public CategoryEntity(string name, int companyId)
        {
            Name = name;
            CompanyId = companyId;
        }
    }
}
