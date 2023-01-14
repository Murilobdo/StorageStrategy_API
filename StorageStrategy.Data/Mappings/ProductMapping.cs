using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Migrations;
using StorageStrategy.Models;

namespace StorageStrategy.Data.Mappings
{
    internal class ProductMapping : IEntityTypeConfiguration<ProductEntity>
    {
        public void Configure(EntityTypeBuilder<ProductEntity> builder)
        {
            builder.HasKey(p => p.ProductId);

            builder.Property(p => p.Name)
                .HasMaxLength(100);

            builder.Property(p => p.Cost)
                .HasPrecision(10, 2);

            builder.Property(p => p.Price)
                .HasPrecision(10, 2);
        }
    }
}
