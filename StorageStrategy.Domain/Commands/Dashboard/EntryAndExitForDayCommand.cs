using StorageStrategy.Domain.Validations.Dashboard;
using StorageStrategy.Models;

namespace StorageStrategy.Domain.Commands.Dashboard
{
    public record class EntryAndExitForDayCommand : CommandBase, IValidation
    {
        public EntryAndExitForDayCommand() 
        {
        
        }

        public EntryAndExitForDayCommand(int companyId, int month)
        {
            CompanyId = companyId;
            Month = month;
        }

        public int CompanyId { get; set; }
        public int DayOfMonth { get; set; }
        public int Month { get; set; }
        public decimal MoneyIn { get; set; }
        public decimal MoneyOut { get; set; }

        public List<Error> GetErros() => new EntryAndExitForDayValidation().Validate(this)
          .Errors.Select(p => new Error(p.ErrorMessage)).ToList();
        public bool IsValid() => new EntryAndExitForDayValidation().Validate(this).IsValid;
    }
}
