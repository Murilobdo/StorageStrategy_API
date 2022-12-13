using System;

namespace StorageStrategy.Models
{
    public class Product 
    {
        public int ProductId { get; set; }
        public int Name { get; set; }
        public decimal Cost { get; set; }
        public decimal Price { get; set; }
        public decimal Qtd { get; set; }
        public bool IsActive { get; set; }
        public virtual CategoryEntity Category { get; set; } = new();
        public int CategoryId { get; set; }
        public virtual Company Company { get; set; } = new();
        public int CompanyId { get; set; }

        public Product()
        {

        }

        //public Product(int productId, int name, decimal cost, decimal price, decimal qtd, bool isActive, int categoryId)
        //{
        //    ProductId = productId;
        //    Name = name;
        //    Cost = cost;
        //    Price = price;
        //    Qtd = qtd;
        //    IsActive = isActive;
        //    CategoryId = categoryId;
        //}
    }
}
