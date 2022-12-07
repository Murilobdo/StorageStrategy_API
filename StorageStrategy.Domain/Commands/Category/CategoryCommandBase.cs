using StorageStrategy.Models;

namespace StorageStrategy.Domain.Commands.Category
{
    public record class CategoryCommandBase : CommandBase
    {
        public int CategoryId { get; set; } = 0;
        public string Name { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public int CompanyId { get; set; } = 0;
    }
}
