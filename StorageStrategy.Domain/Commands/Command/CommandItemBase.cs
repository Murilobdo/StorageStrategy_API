namespace StorageStrategy.Domain.Commands.Command
{
    public class CommandItemBase
    {
        public int CommandItemId { get; set; }
        public int CommandId { get; set; }
        public int ProductId { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Cost { get; set; }
        public decimal Price { get; set; }
        public int Qtd { get; set; }
    }
}
