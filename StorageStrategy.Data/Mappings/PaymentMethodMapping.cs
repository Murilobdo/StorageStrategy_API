using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StorageStrategy.Models;

namespace StorageStrategy.Data.Mappings;

public class PaymentMethodMapping : IEntityTypeConfiguration<PaymentMethodEntity>
{
    public void Configure(EntityTypeBuilder<PaymentMethodEntity> builder)
    {
        builder.HasKey(p => p.PaymentMethodId);
        
        builder.HasIndex(p => new { p.CompanyId, p.Company, p.Method }).IsUnique();
    }
}