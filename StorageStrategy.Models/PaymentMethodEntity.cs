namespace StorageStrategy.Models;

public class PaymentMethodEntity
{
    public int PaymentMethodId { get; set; }
    public int CompanyId { get; set; }
    public string Company { get; set; } = string.Empty;
    public PaymentEnum Method { get; set; }
    public decimal TotalFee { get; set; }
    public bool IsActive { get; set; }
}