using StorageStrategy.Domain.Validations.Command;
using StorageStrategy.Models;

namespace StorageStrategy.Domain.Commands.Command
{
    public record class FinishCommandCommand : CommandBase, IValidation
    {
        public FinishCommandCommand(int commandId, PaymentEnum payment, int companyId)
        {
            CommandId = commandId;
            Payment = payment;
            CompanyId = companyId;
        }

        public int CommandId { get; set; }
        public int CompanyId { get; set; }
        public PaymentEnum Payment { get; set; }

        public List<Error> GetErros() => new FinishCommandValidation().Validate(this)
            .Errors.Select(p => new Error(p.ErrorMessage)).ToList();

        public bool IsValid() => new FinishCommandValidation().Validate(this).IsValid;
    }
}
