namespace StorageStrategy.Domain.Commands.PixCharge;

public record GetPixChargeStatusQuery : CommandBase
{
    public int PixChargeId { get; set; }
    public int CompanyId { get; set; }

    public GetPixChargeStatusQuery(int pixChargeId, int companyId)
    {
        PixChargeId = pixChargeId;
        CompanyId = companyId;
    }
}
