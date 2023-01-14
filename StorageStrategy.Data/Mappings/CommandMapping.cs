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

            builder.HasMany(p => p.Products)
                .WithOne(p => p.Command)
                .HasForeignKey(p => p.CommandId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.Employee)
                .WithOne();
        }
    }
}
