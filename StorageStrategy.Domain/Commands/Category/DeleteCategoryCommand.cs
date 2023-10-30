using StorageStrategy.Domain.Validations.Category;
using StorageStrategy.Models;

namespace StorageStrategy.Domain.Commands.Category
{
    public record class DeleteCategoryCommand : CategoryCommandBase, IValidation
    {
        public DeleteCategoryCommand(int categoryId, int companyId)
        {
            CategoryId = categoryId;
            CompanyId = companyId;
        }

        public DeleteCategoryCommand()
        {

        }

        public bool IsValid() => new DeleteCategoryValidation().Validate(this).IsValid;
        public List<Error> GetErros() => new DeleteCategoryValidation().Validate(this)
            .Errors.Select(p => new Error(p.ErrorMessage)).ToList();
    }
}
