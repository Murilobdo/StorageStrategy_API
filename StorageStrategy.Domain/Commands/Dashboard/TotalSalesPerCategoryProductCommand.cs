using StorageStrategy.Domain.Validations.Dashboard;
using StorageStrategy.Models;
using StorageStrategy.Models.ViewModels;

namespace StorageStrategy.Domain.Commands.Dashboard
{
    public record class TotalSalesPerCategoryProductCommand : DashboardCommandBase
    {
        public List<TotalSalesCategoryViewModel> TotalSalesPerCategory { get; set; } = new();

        public List<Error> GetErros() => new TotalSalesPerCategoryProductValidation().Validate(this)
           .Errors.Select(p => new Error(p.ErrorMessage)).ToList();

        public bool IsValid() => new TotalSalesPerCategoryProductValidation().Validate(this).IsValid;
    }
}
