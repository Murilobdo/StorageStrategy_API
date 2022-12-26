namespace StorageStrategy.Domain.Validations.Category
{
    public class DeleteCategoryValidation : CategoryBaseValidation
    {
        public DeleteCategoryValidation()
        {
            ValidationId();
            ValidationCompanyId();
        }
    }
}
