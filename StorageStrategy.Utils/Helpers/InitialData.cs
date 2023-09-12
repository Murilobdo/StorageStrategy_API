using StorageStrategy.Models;
using System.ComponentModel.Design;

namespace StorageStrategy.Utils.Helpers
{
    public static class InitialData
    {
        public static List<CategoryEntity> GetProductCategories(int companyId) 
        {
            return new List<CategoryEntity>
            { 
                new CategoryEntity("", companyId),
                new CategoryEntity("", companyId),
                new CategoryEntity("", companyId),
                new CategoryEntity("", companyId),
                new CategoryEntity("", companyId),
                new CategoryEntity("", companyId),
                new CategoryEntity("", companyId),
                new CategoryEntity("", companyId),
                new CategoryEntity("", companyId)
            };
        }

        public static List<ExpensesTypeEntity> GetExpensesCategories(int companyId)
        {
            return new List<ExpensesTypeEntity>
            {
                new ExpensesTypeEntity("Despesas Operacionais", companyId),
                new ExpensesTypeEntity("Despesas Financeiras", companyId),
                new ExpensesTypeEntity("Despesas não Operacionais", companyId),
            };
        }
    }
}
