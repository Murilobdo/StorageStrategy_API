namespace StorageStrategy.Domain.Commands.PaymentMethod;

public record PaymentCommandBase : CommandBase
{

    public int PaymentMethodId { get; set; }
    public int CompanyId { get; set; }
    public string Company { get; set; }
    public decimal DebitFee { get; set; }
    public decimal CreditFee { get; set; }
    public bool IsActive { get; set; }
}