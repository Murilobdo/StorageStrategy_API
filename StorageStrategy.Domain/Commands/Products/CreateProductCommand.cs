using StorageStrategy.Domain.Validations.Product;
using StorageStrategy.Models;

namespace StorageStrategy.Domain.Commands.Products
{
    public record class CreateProductCommand : ProductCommandBase, IValidation
    {
        public CreateProductCommand(int productId, string name, decimal cost, decimal price, int qtd, int stockAlert, bool isActive, int categoryId, int companyId)
        {
            ProductId = productId;
            Name = name;
            Cost = cost;
            Price = price;
            Qtd = qtd;
            StockAlert = stockAlert;
            IsActive = isActive;
            CategoryId = categoryId;
            CompanyId = companyId;
        }

        public CreateProductCommand()
        {
        }

        public bool IsValid() => new CreateProductValidation().Validate(this).IsValid;
        public List<Error> GetErros() => new CreateProductValidation().Validate(this)
            .Errors.Select(p => new Error(p.PropertyName, p.ErrorMessage)).ToList();
    }
}
