namespace StorageStrategy.Domain.Validations.Category
{
    public class CreateCategoryValidation : CategoryBaseValidation
    {
        public CreateCategoryValidation()
        {
            validationCompanyId();
            validationName();
        }
    }
}
