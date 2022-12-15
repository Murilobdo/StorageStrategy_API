using StorageStrategy.Models;

namespace StorageStrategy.Domain.Commands.Category
{
    public record class CategoryCommandBase : CommandBase
    {
        public CategoryCommandBase(int categoryId, string name, bool isActive, int companyId)
        {
            CategoryId = categoryId;
            Name = name;
            IsActive = isActive;
            CompanyId = companyId;
        }

        public CategoryCommandBase()
        {
            IsActive = true;
        }

        public int CategoryId { get; set; } = 0;
        public string Name { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public int CompanyId { get; set; } = 0;
    }
}
