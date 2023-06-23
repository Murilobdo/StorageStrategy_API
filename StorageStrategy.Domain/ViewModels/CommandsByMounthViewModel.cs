using StorageStrategy.Models;

namespace StorageStrategy.Domain.ViewModels
{
    public class ImportProductViewModel
    {
        public string Name { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Cost { get; set; } = string.Empty;
        public string Price { get; set; } = string.Empty;
        public string Qtd { get; set; } = string.Empty;
        public string StockAlert { get; set; } = string.Empty;
        public int CompanyId { get; set; }
    }
}