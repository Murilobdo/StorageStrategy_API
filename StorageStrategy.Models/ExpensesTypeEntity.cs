namespace StorageStrategy.Models
{
    public class ExpensesTypeEntity
    {
        public ExpensesTypeEntity() 
        {

        }

        public ExpensesTypeEntity(string description, int companyId)
        {
            CompanyId = companyId;
            Description = description;
        }

        public int ExpenseTypeId { get; set; }
        public int CompanyId { get; set; }
        public virtual CompanyEntity Company { get; set; }
        public string Description { get; set; }
    }
}
