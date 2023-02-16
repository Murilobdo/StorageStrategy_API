namespace StorageStrategy.Models
{
    public class ExpensesEntity
    {
        public ExpensesEntity() 
        {

        }

        public int ExpenseId { get; set; }

        public int CompanyId { get; set; }

        public virtual CompanyEntity Company { get; set; }

        public string Description { get; set; } = string.Empty;

        public int ExpensesTypeId { get; set; }

        public virtual ExpensesTypeEntity ExpensesType { get; set; }

        public DateTime CreateAt { get; set; }
    }
}
