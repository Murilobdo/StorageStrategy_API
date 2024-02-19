
using FluentValidation;
using StorageStrategy.Domain.Validations.StockHsitory;
using StorageStrategy.Models;

namespace StorageStrategy.Domain.Commands.StockHistory
{
    public class CreateStockHsitoryCommand : IValidation
    {
        public int CompanyId { get; set; }
        public List<StockHistoryItem> Products { get; set; } = new();

        public List<Error> GetErros()
        {
           return new CreateStockHsitoryValidation()
                .Validate(this)
                .Errors.Select(p => new Error(p.ErrorMessage)).ToList();
        }

        public bool IsValid()
        {
            return new CreateStockHsitoryValidation().Validate(this).IsValid;
        }
    }
}
