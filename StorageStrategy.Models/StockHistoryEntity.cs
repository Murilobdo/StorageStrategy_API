
namespace StorageStrategy.Models
{
    public record StockHistoryEntity
    {
        public StockHistoryEntity(int companyId, List<StockHistoryItemEntity> products)
        {
            CompanyId = companyId;
            Products = products;
            CreateAt = DateTime.Now;
        }

        public StockHistoryEntity()
        {
            
        }

        public int StockHistoryId { get; set; }
        public int CompanyId { get; set; }
        public CompanyEntity Company { get; set; } = new();
        public DateTime CreateAt { get; set; }
        public List<StockHistoryItemEntity> Products { get; set; } = new();

    }
}
