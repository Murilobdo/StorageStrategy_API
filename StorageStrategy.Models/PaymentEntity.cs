using System.Security.AccessControl;

namespace StorageStrategy.Models;

public class PaymentEntity
{
    public int PaymentId { get; set; }
    public int CommandId { get; set; }
    public CommandEntity Command { get; set; } = new();
    public PaymentEnum Method { get; set; }
    public decimal Amount { get; set; }
    public decimal TotalFee { get; set; }
    public decimal AmountWithFee { get; set; }
    public int? PaymentMethodId { get; set; }
    public PaymentMethodEntity PaymentMethod { get; set; }

    public PaymentEntity(
        int paymentId, 
        int commandId, 
        PaymentEnum method, 
        decimal amount,
        int paymentMethodId,
        decimal totalFee
    ) {
        PaymentId = paymentId;
        CommandId = commandId;
        Method = method;
        Amount = amount;
        PaymentMethodId = paymentMethodId;
        TotalFee = totalFee;
        AmountWithFee = amount - (amount * totalFee / 100);
    }

    public PaymentEntity()
    {
        
    }
}