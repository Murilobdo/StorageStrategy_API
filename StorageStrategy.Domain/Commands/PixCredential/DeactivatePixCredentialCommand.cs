namespace StorageStrategy.Domain.Commands.PixCredential;

public record DeactivatePixCredentialCommand : CommandBase
{
    public int CompanyId { get; set; }

    public DeactivatePixCredentialCommand(int companyId)
    {
        CompanyId = companyId;
    }
}
