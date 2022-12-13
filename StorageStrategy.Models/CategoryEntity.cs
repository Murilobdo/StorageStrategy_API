
namespace StorageStrategy.Models
{
    public class CategoryEntity
    {
        public int CategoryId { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public List<Product> Products { get; set; } = new();
        public virtual Company Company { get; set; }
        public int CompanyId { get; set; }

        public CategoryEntity()
        {

        }
    }
}
