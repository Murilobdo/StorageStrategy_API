namespace StorageStrategy.Domain.Validations.Category
{
    public class UpdateCategoryValidation : CategoryBaseValidation
    {
        public UpdateCategoryValidation()
        {
            validationId();
            validationCompanyId();
            validationName();
        }
    }
}
