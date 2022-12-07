using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Migrations;
using StorageStrategy.Models;

namespace StorageStrategy.Data.Mappings
{
    internal class ProductMapping : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(p => p.ProductId);

            builder.Property(p => p.Name)
                .HasMaxLength(100);

            builder.Property(p => p.Cost)
                .HasPrecision(2);

            builder.Property(p => p.Price)
                .HasPrecision(2);

        }
    }
}
