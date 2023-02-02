using StorageStrategy.Domain.Commands;
using System.Security.Cryptography;

namespace StorageStrategy.Domain.Commands.Dashboard
{
    public record class EntryAndExitOfMonthCommand : CommandBase
    {
        public EntryAndExitOfMonthCommand(int companyId)
        {
            CompanyId = companyId;
        }

        public int CompanyId { get; set; }
        public DateTime CurrentDate { get; set; }
        public decimal MoneyIn { get; set; } = decimal.Zero;
        public decimal MoneyOut { get; set; } = decimal.Zero;
    }
}
