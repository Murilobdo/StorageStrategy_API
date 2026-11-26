using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StorageStrategy.Models;

namespace StorageStrategy.Data.Mappings;

public class PrintStationMapping : IEntityTypeConfiguration<PrintStationEntity>
{
    public void Configure(EntityTypeBuilder<PrintStationEntity> builder)
    {
        builder.ToTable("PrintStation");
        builder.HasKey(p => p.PrintStationId);

        builder.Property(p => p.Name)
            .HasColumnType("nvarchar(60)")
            .IsRequired();

        builder.Property(p => p.KeyHash)
            .HasColumnType("varbinary(32)")
            .IsRequired();

        builder.Property(p => p.KeyLastChars)
            .HasColumnType("nvarchar(4)")
            .IsRequired();

        // Uma estação ativa por empresa.
        builder.HasIndex(p => p.CompanyId)
            .IsUnique()
            .HasFilter("[IsActive] = 1");
    }
}
