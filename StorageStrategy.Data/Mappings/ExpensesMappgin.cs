using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StorageStrategy.Models;

namespace StorageStrategy.Data.Mappings
{
    internal class ExpensesMappgin : IEntityTypeConfiguration<ExpensesEntity>
    {
        public void Configure(EntityTypeBuilder<ExpensesEntity> builder)
        {
            builder.HasKey(p => p.ExpenseId);
        }
    }
}
