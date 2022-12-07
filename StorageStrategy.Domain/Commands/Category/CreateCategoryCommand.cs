using StorageStrategy.Domain.Validations.Category;
using StorageStrategy.Models;

namespace StorageStrategy.Domain.Commands.Category
{
    public record class CreateCategoryCommand : CategoryCommandBase, IValidation
    {
        public bool IsValid() => new CreateCategoryValidation().Validate(this).IsValid;
        public List<Error> GetErros() => new CreateCategoryValidation().Validate(this)
            .Errors.Select(p => new Error(p.PropertyName, p.ErrorMessage)).ToList();
    }
}
