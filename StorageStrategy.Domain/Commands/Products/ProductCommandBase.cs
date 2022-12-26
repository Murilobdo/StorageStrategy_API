using StorageStrategy.Models;

namespace StorageStrategy.Domain.Commands.Products
{
    public record class ProductCommandBase : CommandBase
    {
        public ProductCommandBase()
        {

        }

        public ProductCommandBase(int productId, string name, decimal cost, decimal price, decimal qtd, bool isActive, int categoryId, int companyId)
        {
            ProductId = productId;
            Name = name;
            Cost = cost;
            Price = price;
            Qtd = qtd;
            IsActive = isActive;
            CategoryId = categoryId;
            CompanyId = companyId;
        }

        public int ProductId { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Cost { get; set; }
        public decimal Price { get; set; }
        public decimal Qtd { get; set; }
        public bool IsActive { get; set; }
        public int CategoryId { get; set; }
        public int CompanyId { get; set; }
    }
}
