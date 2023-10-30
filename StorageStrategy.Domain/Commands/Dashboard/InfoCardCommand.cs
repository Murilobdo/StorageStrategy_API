using StorageStrategy.Domain.Validations.Dashboard;
using StorageStrategy.Models;

namespace StorageStrategy.Domain.Commands.Dashboard
{
    public record class InfoCardCommand : CommandBase
    {
        public InfoCardCommand()
        { }

        public InfoCardCommand(int companyId, int month, int qtdProducts, decimal totalSales, decimal totalPriceInStok)
        {
            CompanyId = companyId;
            Month = month;
            QtdProducts = qtdProducts;
            TotalSales = totalSales;
            TotalPriceInStok = totalPriceInStok;
        }
        
        public int CompanyId { get; set; }
        public int Month { get; set; }
        public int QtdProducts  { get; set; }
        public decimal TotalSales { get; set; }
        public decimal TotalPriceInStok { get; set; }

        public List<Error> GetErros() => new InfoCardValidation().Validate(this)
           .Errors.Select(p => new Error(p.ErrorMessage)).ToList();

        public bool IsValid() => new InfoCardValidation().Validate(this).IsValid;
    }
}
