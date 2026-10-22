using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StorageStrategy.Models;

namespace StorageStrategy.Data.Mappings;

public class PixChargeMapping : IEntityTypeConfiguration<PixChargeEntity>
{
    public void Configure(EntityTypeBuilder<PixChargeEntity> builder)
    {
        builder.ToTable("PixCharge");
        builder.HasKey(p => p.PixChargeId);

        builder.Property(p => p.QrCodeBase64)
            .HasColumnType("nvarchar(max)");

        builder.HasIndex(p => p.CompanyId);
        builder.HasIndex(p => p.CommandId);
        builder.HasIndex(p => p.MpPaymentId).IsUnique();
    }
}
