using StorageStrategy.Domain.Validations.PixCredential;
using StorageStrategy.Models;

namespace StorageStrategy.Domain.Commands.PixCredential;

public record SavePixCredentialCommand : CommandBase, IValidation
{
    public int CompanyId { get; set; }
    public int EmployeeId { get; set; }
    public string AccessToken { get; set; } = string.Empty;
    public string WebhookSecret { get; set; } = string.Empty;

    public bool IsValid()
    {
        return new SavePixCredentialValidation().Validate(this).IsValid;
    }

    public List<Error> GetErros()
    {
        return new SavePixCredentialValidation().Validate(this)
            .Errors.Select(p => new Error(p.ErrorMessage)).ToList();
    }
}
