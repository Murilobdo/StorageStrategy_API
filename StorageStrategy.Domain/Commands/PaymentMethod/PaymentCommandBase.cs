namespace StorageStrategy.Domain.Commands.Payment;

public record PaymentCommandBase : CommandBase
{

    public int PaymentMethodId { get; set; }
    public int CompanyId { get; set; }
    public string Name { get; set; }
    public string Company { get; set; }
    public int DebitFee { get; set; }
    public int CreditFee { get; set; }
    public bool IsActive { get; set; }
}