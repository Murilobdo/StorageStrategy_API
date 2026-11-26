using StorageStrategy.Domain.Validations.PixCharge;
using StorageStrategy.Models;

namespace StorageStrategy.Domain.Commands.PixCharge;

public record CreatePixChargeCommand : CommandBase, IValidation
{
    public int CommandId { get; set; }
    public int CompanyId { get; set; }

    public bool IsValid()
    {
        return new CreatePixChargeValidation().Validate(this).IsValid;
    }

    public List<Error> GetErros()
    {
        return new CreatePixChargeValidation().Validate(this)
            .Errors.Select(p => new Error(p.ErrorMessage)).ToList();
    }
}
