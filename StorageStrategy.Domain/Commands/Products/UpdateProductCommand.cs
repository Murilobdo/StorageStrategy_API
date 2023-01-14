using StorageStrategy.Domain.Validations.Category;
using StorageStrategy.Domain.Validations.Product;
using StorageStrategy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageStrategy.Domain.Commands.Products
{
    public record class UpdateProductCommand : ProductCommandBase, IValidation
    {
        public UpdateProductCommand(int productId, string name, decimal cost, decimal price, int qtd, int stockAlert, bool isActive, int categoryId, int companyId)
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

        public UpdateProductCommand()
        {
        }

        public bool IsValid() => new UpdateProductValidation().Validate(this).IsValid;
        public List<Error> GetErros() => new UpdateProductValidation().Validate(this)
            .Errors.Select(p => new Error(p.PropertyName, p.ErrorMessage)).ToList();
    }
}
