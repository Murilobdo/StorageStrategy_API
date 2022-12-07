using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StorageStrategy.Models;

namespace StorageStrategy.Data.Mappings
{
    public class CompanyMapping : IEntityTypeConfiguration<Company>
    {
        public void Configure(EntityTypeBuilder<Company> builder)
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
        }
    }
}
