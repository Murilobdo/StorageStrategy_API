using StorageStrategy.Domain.Commands;
using StorageStrategy.Domain.Validations.Command;
using StorageStrategy.Domain.Validations.Dashboard;
using StorageStrategy.Models;
using System.Security.Cryptography;

namespace StorageStrategy.Domain.Commands.Dashboard
{
    public record class EntryAndExitOfMonthCommand : CommandBase, IValidation
    {
        public EntryAndExitOfMonthCommand(int companyId, DateTime currentDate)
        {
            CompanyId = companyId;
            CurrentDate = currentDate;
        }

        public int CompanyId { get; set; }
        public DateTime CurrentDate { get; set; }
        public decimal MoneyIn { get; set; } = decimal.Zero;
        public decimal MoneyOut { get; set; } = decimal.Zero;

        public List<Error> GetErros() => new EntryAndExitOfMonthValidation().Validate(this)
           .Errors.Select(p => new Error(p.ErrorMessage)).ToList();

        public bool IsValid() => new EntryAndExitOfMonthValidation().Validate(this).IsValid;
    }
}
