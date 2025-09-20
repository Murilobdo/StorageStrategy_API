using StorageStrategy.Domain.Validations.Dashboard;
using StorageStrategy.Models;

namespace StorageStrategy.Domain.Commands.Dashboard
{
    public record class EntryAndExitOfMonthCommand : DashboardCommandBase, IValidation
    {
        public EntryAndExitOfMonthCommand(int companyId, DateTime currentDate)
        {
            CompanyId = companyId;
            CurrentDate = currentDate;
        }

        public DateTime CurrentDate { get; set; }
        public decimal MoneyIn { get; set; } = decimal.Zero;
        public decimal MoneyOut { get; set; } = decimal.Zero;

        public List<Error> GetErros() => new EntryAndExitOfMonthValidation().Validate(this)
           .Errors.Select(p => new Error(p.ErrorMessage)).ToList();

        public bool IsValid() => new EntryAndExitOfMonthValidation().Validate(this).IsValid;
    }
}
