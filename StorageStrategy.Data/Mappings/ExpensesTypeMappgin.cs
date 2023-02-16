using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StorageStrategy.Models;

namespace StorageStrategy.Data.Mappings
{
    internal class ExpensesTypeMappgin : IEntityTypeConfiguration<ExpensesTypeEntity>
    {
        public void Configure(EntityTypeBuilder<ExpensesTypeEntity> builder)
        {
            builder.HasKey(p => p.ExpenseTypeId);
        }
    }
}
