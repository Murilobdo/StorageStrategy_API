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

            builder.HasMany(p => p.Items)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(p => p.Employee)
                .WithMany();
        }
    }
}
