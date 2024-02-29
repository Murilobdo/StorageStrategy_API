using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StorageStrategy.Models;

namespace StorageStrategy.Data.Mappings
{
    public class CompanyMapping : IEntityTypeConfiguration<CompanyEntity>
    {
        public void Configure(EntityTypeBuilder<CompanyEntity> builder)
        {
            builder.HasKey(p => p.CompanyId);

            builder.HasMany(p => p.Products)
              .WithOne(p => p.Company)
              .HasForeignKey(p => p.CompanyId)
              .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(p => p.Categorys)
              .WithOne(p => p.Company)
              .HasForeignKey(p => p.CompanyId)
              .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(p => p.Employees)
                .WithOne(p => p.Company)
                .HasForeignKey(p => p.CompanyId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(p => p.Commands)
                .WithOne(p => p.Company)
                .HasForeignKey(p => p.CompanyId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(p => p.Expenses)
                .WithOne(p => p.Company)
                .HasForeignKey(p => p.CompanyId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(p => p.StockHistory)
                .WithOne(p => p.Company)
                .HasForeignKey(p => p.CompanyId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
