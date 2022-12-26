namespace StorageStrategy.Domain.Validations.Category
{
    public class CreateCategoryValidation : CategoryBaseValidation
    {
        public CreateCategoryValidation()
        {
            ValidationCompanyId();
            ValidationName();
        }
    }
}
