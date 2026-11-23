namespace StorageStrategy.Domain.Commands.PixCredential;

public record GetPixCredentialStatusQuery : CommandBase
{
    public int CompanyId { get; set; }

    public GetPixCredentialStatusQuery(int companyId)
    {
        CompanyId = companyId;
    }
}
