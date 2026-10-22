using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StorageStrategy.Models;

namespace StorageStrategy.Data.Mappings;

public class PixCredentialMapping : IEntityTypeConfiguration<PixCredentialEntity>
{
    public void Configure(EntityTypeBuilder<PixCredentialEntity> builder)
    {
        builder.ToTable("PixCredential");
        builder.HasKey(p => p.PixCredentialId);

        builder.Property(p => p.AccessTokenEncrypted)
            .HasColumnType("nvarchar(max)");

        builder.Property(p => p.WebhookSecretEncrypted)
            .HasColumnType("nvarchar(max)");

        builder.HasIndex(p => p.CompanyId).IsUnique();
    }
}
