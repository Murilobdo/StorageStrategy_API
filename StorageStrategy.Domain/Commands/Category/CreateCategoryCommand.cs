using StorageStrategy.Domain.Validations.Category;
using StorageStrategy.Models;

namespace StorageStrategy.Domain.Commands.Category
{
    public record class CreateCategoryCommand : CategoryCommandBase, IValidation
    {
        public CreateCategoryCommand(int categoryId, string name, bool isActive, int companyId)
        {
            CategoryId = categoryId;
            Name = name;
            IsActive = isActive;
            CompanyId = companyId;
        }

        public CreateCategoryCommand()
        {

        }

        public bool IsValid() => new CreateCategoryValidation().Validate(this).IsValid;
        public List<Error> GetErros() => new CreateCategoryValidation().Validate(this)
            .Errors.Select(p => new Error(p.ErrorMessage)).ToList();
    }
}
