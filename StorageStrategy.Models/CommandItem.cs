namespace StorageStrategy.Models
{
    public class CommandItem
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

        public CommandItem()
        {

        }
    }
}
