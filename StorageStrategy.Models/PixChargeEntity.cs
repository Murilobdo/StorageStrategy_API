namespace StorageStrategy.Models;

public class PixChargeEntity
{
    public int PixChargeId { get; set; }
    public int CompanyId { get; set; }
    public int CommandId { get; set; }
    public long? MpPaymentId { get; set; }
    public PixChargeStatusEnum Status { get; set; }
    public decimal Valor { get; set; }
    public string? QrCode { get; set; }
    public string? QrCodeBase64 { get; set; }
    public string? TicketUrl { get; set; }
    public DateTime ExpiraEm { get; set; }
    public DateTime CriadoEm { get; set; }
    public DateTime AtualizadoEm { get; set; }

    public PixChargeEntity()
    {
        CriadoEm = DateTime.Now;
        AtualizadoEm = DateTime.Now;
        Status = PixChargeStatusEnum.Criando;
    }
}
