namespace StorageStrategy.Models
{
    public class ExpensesTypeEntity
    {
        public ExpensesTypeEntity() 
        {

        }

        public int ExpenseTypeId { get; set; }
        public int CompanyId { get; set; }
        public virtual CompanyEntity Company { get; set; }
        public string Description { get; set; }
    }
}
