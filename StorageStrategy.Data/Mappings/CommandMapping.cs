using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StorageStrategy.Models;

namespace StorageStrategy.Data.Mappings
{
    public class CommandMapping : IEntityTypeConfiguration<CommandEntity>
    {
        public void Configure(EntityTypeBuilder<CommandEntity> builder)
        {
            builder.HasKey(p => p.CommandId);
            
            // builder.HasIndex(p => new {p.CompanyId, p.Code})
            //     .IsUnique();
            
            
            builder.HasMany(p => p.Items)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(p => p.Employee)
                .WithMany();

            builder.Property(p => p.ClientId)
                .IsRequired(false);
            
            builder.HasOne(p => p.Client)
                .WithMany(p => p.Commands)
                .HasForeignKey(p => p.ClientId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
