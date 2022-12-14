using System;

namespace StorageStrategy.Models
{
    public class ProductEntity 
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public decimal Cost { get; set; }
        public decimal Price { get; set; }
        public decimal Qtd { get; set; }
        public bool IsActive { get; set; }
        public virtual CategoryEntity Category { get; set; }
        public int CategoryId { get; set; }
        public virtual CompanyEntity Company { get; set; }
        public int CompanyId { get; set; }

        public ProductEntity()
        {

        }

        public ProductEntity(int productId, string name, decimal cost, decimal price, decimal qtd, bool isActive, int categoryId)
        {
            ProductId = productId;
            Name = name;
            Cost = cost;
            Price = price;
            Qtd = qtd;
            IsActive = isActive;
            CategoryId = categoryId;
        }
    }
}
