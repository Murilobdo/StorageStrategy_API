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
        public virtual ExpensesTypeEntity ExpensesType { get; set; }
        public int CompanyId { get; set; }
        public virtual CompanyEntity Company { get; set; }
    }
}
