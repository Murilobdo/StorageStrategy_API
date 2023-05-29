using StorageStrategy.Domain.Validations.Product;
using StorageStrategy.Models;

namespace StorageStrategy.Domain.Commands.Products
{
    public record class DeleteProductCommand : ProductCommandBase, IValidation
    {
        public DeleteProductCommand(int productId, int companyId)
        {
            ProductId = productId;
            CompanyId = companyId;
        }

        public DeleteProductCommand()
        {

        }

        public bool IsValid() => new DeleteProductValidation().Validate(this).IsValid;
        public List<Error> GetErros() => new DeleteProductValidation().Validate(this)
            .Errors.Select(p => new Error(p.PropertyName, p.ErrorMessage)).ToList();
    }
}
