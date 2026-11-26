using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StorageStrategy.Models;

namespace StorageStrategy.Data.Mappings;

public class PrintJobMapping : IEntityTypeConfiguration<PrintJobEntity>
{
    public void Configure(EntityTypeBuilder<PrintJobEntity> builder)
    {
        builder.ToTable("PrintJob");
        builder.HasKey(p => p.PrintJobId);

        builder.Property(p => p.Payload)
            .HasColumnType("varbinary(max)")
            .IsRequired();

        builder.Property(p => p.FailureReason)
            .HasColumnType("nvarchar(200)");

        builder.Ignore(p => p.IsFinal);

        // Claim e expiração.
        builder.HasIndex(p => new { p.CompanyId, p.Status, p.CreatedAt });
        // Último status da comanda e cálculo da via.
        builder.HasIndex(p => new { p.CommandId, p.CreatedAt });
    }
}
