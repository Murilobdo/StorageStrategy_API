using StorageStrategy.Domain.Validations.Product;
using StorageStrategy.Models;

namespace StorageStrategy.Domain.Commands.Products
{
    public record class UpdateProductCommand : ProductCommandBase, IValidation
    {
        public UpdateProductCommand(int productId, string name, decimal cost, decimal price, int qtd, int stockAlert, bool isActive, int categoryId, int companyId, int taxing)
        {
            ProductId = productId;
            Name = name;
            Cost = cost;
            Price = price;
            Qtd = qtd;
            StockAlert = stockAlert;
            IsActive = qtd > 0;
            Taxing = taxing;
            CategoryId = categoryId;
            CompanyId = companyId;
        }

        public UpdateProductCommand()
        {
        }

        public bool IsValid() => new UpdateProductValidation().Validate(this).IsValid;
        public List<Error> GetErros() => new UpdateProductValidation().Validate(this)
            .Errors.Select(p => new Error(p.ErrorMessage)).ToList();
    }
}
