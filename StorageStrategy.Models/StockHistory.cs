
namespace StorageStrategy.Models
{
    public record StockHistory(int CompanyId, DateTime CreateAt, List<StockHistoryItem> Products)
    {
        public int CompanyId { get; set; } = CompanyId;
        public DateTime CreateAt { get; set; } = CreateAt;
        public List<StockHistoryItem> Products { get; set; } = Products ?? new();

    
    }
}
