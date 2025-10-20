using StorageStrategy.Models;

namespace StorageStrategy.Domain.Commands.PaymentMethod;

public record PaymentCommandBase : CommandBase
{

    public int PaymentMethodId { get; set; }
    public int CompanyId { get; set; }
    public string Company { get; set; }
    public PaymentEnum Method { get; set; }
    public decimal TotalFee { get; set; }
    public bool IsActive { get; set; }
}