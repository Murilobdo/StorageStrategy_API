namespace StorageStrategy.Models
{
    public class ExpenseEntity
    {
        public ExpenseEntity() 
        {

        }

        public int ExpenseId { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal ExpenseValue { get; set; }
        public DateTime CreateAt { get; set; }
        public int ExpensesTypeId { get; set; }
        public ExpensesTypeEntity ExpensesType { get; set; }
        public int CompanyId { get; set; }
        public CompanyEntity Company { get; set; }
    }
}
