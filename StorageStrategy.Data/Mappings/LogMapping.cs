using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StorageStrategy.Models;

namespace StorageStrategy.Data.Mappings;

public class LogMapping  : IEntityTypeConfiguration<LogApp>
{
    public void Configure(EntityTypeBuilder<LogApp> builder)
    {
        builder.ToTable("LogApp");
        builder.HasKey(x => x.LogId);
    }
}