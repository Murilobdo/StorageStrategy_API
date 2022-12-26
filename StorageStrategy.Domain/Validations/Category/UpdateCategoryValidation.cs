namespace StorageStrategy.Domain.Validations.Category
{
    public class UpdateCategoryValidation : CategoryBaseValidation
    {
        public UpdateCategoryValidation()
        {
            ValidationId();
            ValidationCompanyId();
            ValidationName();
        }
    }
}
