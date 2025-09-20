using StorageStrategy.Domain.Validations.Dashboard;
using StorageStrategy.Models;

namespace StorageStrategy.Domain.Commands.Dashboard
{
    public record class InfoCardCommand : DashboardCommandBase
    {
        public InfoCardCommand()
        { }

        public InfoCardCommand(
            int companyId, 
            int month, 
            decimal totalSales, 
            decimal totalProfit
        ) {
            CompanyId = companyId;
            Month = month;
            TotalSales = totalSales;
            TotalProfit = totalProfit;
        }
        
        public decimal TotalSales { get; set; }
        public decimal TotalProfit { get; set; }
        public decimal TotalMonthExpenses { get; set; }

        public List<Error> GetErros() => new InfoCardValidation().Validate(this)
           .Errors.Select(p => new Error(p.ErrorMessage)).ToList();

        public bool IsValid() => new InfoCardValidation().Validate(this).IsValid;
    }
}
