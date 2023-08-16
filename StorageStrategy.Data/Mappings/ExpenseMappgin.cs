using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StorageStrategy.Models;

namespace StorageStrategy.Data.Mappings
{
    public class ExpenseMappgin : IEntityTypeConfiguration<ExpenseEntity>
    {
        public void Configure(EntityTypeBuilder<ExpenseEntity> builder)
        {
            builder.HasKey(p => p.ExpenseId);

            builder.Property(p => p.ExpenseValue)
                .HasPrecision(4);
        }
    }
}
