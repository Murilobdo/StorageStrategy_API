
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StorageStrategy.Models;

namespace StorageStrategy.Data.Mappings
{
    public class StockHistoryMapping : IEntityTypeConfiguration<StockHistoryEntity>
    {
        public void Configure(EntityTypeBuilder<StockHistoryEntity> builder)
        {
            builder.HasKey(p => p.StockHistoryId);
        }
    }
}
