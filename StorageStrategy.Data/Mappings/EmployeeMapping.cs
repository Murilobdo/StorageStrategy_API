using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StorageStrategy.Models;

namespace StorageStrategy.Data.Mappings
{
    internal class EmployeeMapping : IEntityTypeConfiguration<EmployeeEntity>
    {
        public void Configure(EntityTypeBuilder<EmployeeEntity> builder)
        {
            builder.HasKey(p => p.EmployeeId);

            builder.Property(p => p.Name)
                .HasMaxLength(30);

            builder.Property(p => p.JobRole)
                .HasMaxLength(30);

        }
    }
}
