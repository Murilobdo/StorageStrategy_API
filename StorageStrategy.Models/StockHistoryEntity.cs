
namespace StorageStrategy.Models
{
    public record StockHistoryEntity
    {
        public int StockHistoryId { get; set; }
        public int CompanyId { get; set; }
        public CompanyEntity Company { get; set; } = new();
        public DateTime CreateAt { get; set; }
        public List<StockHistoryItemEntity> Products { get; set; } = new();

    }
}
