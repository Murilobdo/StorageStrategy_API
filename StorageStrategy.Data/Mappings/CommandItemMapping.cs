using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StorageStrategy.Models;

namespace StorageStrategy.Data.Mappings
{
    public class CommandItemMapping : IEntityTypeConfiguration<CommandItemEntity>
    {
        public void Configure(EntityTypeBuilder<CommandItemEntity> builder)
        {
            builder.HasKey(p => p.CommandItemId);

            builder.HasOne(p => p.Command)
                .WithMany(p => p.Items)
                .HasForeignKey(p => p.CommandId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
