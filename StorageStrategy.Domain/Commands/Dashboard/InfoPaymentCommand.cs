using StorageStrategy.Domain.Validations.Dashboard;
using StorageStrategy.Models;

namespace StorageStrategy.Domain.Commands.Dashboard
{
    public record class InfoPaymentCommand : CommandBase
    {
        public InfoPaymentCommand() 
        {
        
        }
        public InfoPaymentCommand(int companyId, int month)
        {
            CompanyId = companyId;
            Month = month;
        }

        public int CompanyId { get; set; }
        public int Month { get; set; }
        public decimal Total { get; set; }
        public decimal TotalPix { get; set; }
        public decimal TotalCredit { get; set; }
        public decimal TotalDebit { get; set; }
        public decimal TotalCash { get; set; }

        public List<Error> GetErros() => new InfoPaymentValidation().Validate(this)
           .Errors.Select(p => new Error(p.ErrorMessage)).ToList();
        public bool IsValid() => new InfoPaymentValidation().Validate(this).IsValid;
    }
}
