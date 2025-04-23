namespace StorageStrategy.Models
{
    public class CommandItemEntity
    {
        public int CommandItemId { get; set; }
        public int CommandId { get; set; }
        public CommandEntity Command { get; set; }
        public int ProductId { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Cost { get; set; }
        public decimal Price { get; set; }
        public int Qtd { get; set; }
        public virtual ProductEntity Product { get; set; }
        public decimal Taxing { get; set; }
        public CommandItemEntity()
        {

        }

        public CommandItemEntity(int commandItemId, int commandId, int productId, string name, decimal cost, decimal price, int qtd, decimal taxing)
        {
            CommandItemId = commandItemId;
            CommandId = commandId;
            ProductId = productId;
            Name = name;
            Cost = cost;
            Price = price;
            Qtd = qtd;
            Taxing = taxing;
        }
        
        public CommandItemEntity(int commandId, int productId, string name, decimal cost, decimal price, int qtd)
        {
            CommandId = commandId;
            ProductId = productId;
            Name = name;
            Cost = cost;
            Price = price;
            Qtd = qtd;
        }

       
    }
}
