using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StorageStrategy.Models;

namespace StorageStrategy.Data.Mappings;

public class PaymentMapping : IEntityTypeConfiguration<PaymentEntity> 
{
    public void Configure(EntityTypeBuilder<PaymentEntity> builder)
    {
        builder.ToTable("Payment");
        builder.HasKey(x => x.PaymentId);

        builder.HasOne(p => p.Command)
            .WithMany(p => p.Payments)
            .HasForeignKey(p => p.CommandId);
    }
}