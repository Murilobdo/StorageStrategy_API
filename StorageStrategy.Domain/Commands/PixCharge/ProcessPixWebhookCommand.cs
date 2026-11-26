namespace StorageStrategy.Domain.Commands.PixCharge;

public record ProcessPixWebhookCommand : CommandBase
{
    public int CompanyId { get; set; }
    public long MpPaymentId { get; set; }

    public ProcessPixWebhookCommand(int companyId, long mpPaymentId)
    {
        CompanyId = companyId;
        MpPaymentId = mpPaymentId;
    }
}
