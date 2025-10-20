using StorageStrategy.Domain.Validations.Dashboard;
using StorageStrategy.Models;

namespace StorageStrategy.Domain.Commands.Dashboard
{
    public record class EntryAndExitForDayCommand : DashboardCommandBase, IValidation
    {
        public EntryAndExitForDayCommand() 
        {
        
        }

        public EntryAndExitForDayCommand(int companyId, int month, int year)
        {
            CompanyId = companyId;
            Month = month;
            Year = year;
        }

        public int DayOfMonth { get; set; }
        public decimal MoneyIn { get; set; }
        public decimal MoneyOut { get; set; }

        public List<Error> GetErros() => new EntryAndExitForDayValidation().Validate(this)
          .Errors.Select(p => new Error(p.ErrorMessage)).ToList();
        public bool IsValid() => new EntryAndExitForDayValidation().Validate(this).IsValid;
    }
}
