
namespace StorageStrategy.Domain.Commands.Products
{
    public record ProductCommandBase : CommandBase
    {
        public ProductCommandBase()
        {

        }

        public ProductCommandBase(int productId, string name, decimal cost, decimal price, int qtd, bool isActive, int categoryId, int companyId, decimal taxing)
        {
            ProductId = productId;
            Name = name;
            Cost = cost;
            Price = price;
            Qtd = qtd;
            IsActive = qtd > 0;
            CategoryId = categoryId;
            CompanyId = companyId;
            Taxing = taxing;
        }

        public int ProductId { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Cost { get; set; }
        public decimal Price { get; set; }
        public int Qtd { get; set; }
        public int StockAlert { get; set; }
        public bool IsActive { get; set; }
        public int CategoryId { get; set; }
        public int CompanyId { get; set; }
        public decimal Taxing { get; set; }
        public bool IsService { get; set; }
    }
}
