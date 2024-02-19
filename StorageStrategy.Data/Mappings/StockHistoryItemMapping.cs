
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StorageStrategy.Models;

namespace StorageStrategy.Data.Mappings
{
    public class StockHistoryItemMapping : IEntityTypeConfiguration<StockHistoryItemEntity>
    {
        public void Configure(EntityTypeBuilder<StockHistoryItemEntity> builder)
        {
            builder.HasKey(p => p.StockHistoryItemId);
        }
    }
}
