using StorageStrategy.Domain.Validations.Product;
using StorageStrategy.Models;

namespace StorageStrategy.Domain.Commands.Products
{
    public record class CreateProductCommand : ProductCommandBase, IValidation
    {
        public CreateProductCommand(
            string name, 
            decimal cost, 
            decimal price, 
            int qtd, 
            int stockAlert, 
            bool isActive, 
            int categoryId, 
            int companyId, 
            decimal taxing
        ) {
            Name = name;
            Cost = cost;
            Price = price;
            Qtd = qtd;
            StockAlert = stockAlert;
            IsActive = qtd > 0;
            CategoryId = categoryId;
            CompanyId = companyId;
            Taxing = taxing;
        }

        public CreateProductCommand()
        {
        }

        public bool IsValid() => new CreateProductValidation().Validate(this).IsValid;
        public List<Error> GetErros() => new CreateProductValidation().Validate(this)
            .Errors.Select(p => new Error(p.ErrorMessage)).ToList();
    }
}
