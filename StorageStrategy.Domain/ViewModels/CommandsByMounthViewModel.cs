using StorageStrategy.Models;

namespace StorageStrategy.Domain.ViewModels
{
    public class CommandsByMounthViewModel
    {
        public List<CommandEntity> Commands { get; set; } = new();
        public decimal  TotalCost { get; set; }
        public decimal  TotalPrice { get; set; }
        public decimal TotalLucro => TotalPrice - TotalCost;
    }
}