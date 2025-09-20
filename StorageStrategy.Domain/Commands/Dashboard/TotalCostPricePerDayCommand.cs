using StorageStrategy.Domain.Validations.Dashboard;
using StorageStrategy.Models;

namespace StorageStrategy.Domain.Commands.Dashboard
{
    public record class TotalCostPricePerDayCommand : DashboardCommandBase
    {
        public TotalCostPricePerDayCommand()
        {

        }

        public TotalCostPricePerDayCommand(int companyId, int month, int year)
        {
            CompanyId = companyId;
            Month = month;
            Year = year;
        }

        public List<Error> GetErros() => new TotalCostPricePerDayValidation().Validate(this)
           .Errors.Select(p => new Error(p.ErrorMessage)).ToList();

        public bool IsValid() => new TotalCostPricePerDayValidation().Validate(this).IsValid;
    }
}
