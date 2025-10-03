using System.Security.AccessControl;

namespace StorageStrategy.Models;

public class PaymentEntity
{
    public int PaymentId { get; set; }
    public int CommandId { get; set; }
    public CommandEntity Command { get; set; } = new();
    public PaymentEnum Method { get; set; }
    public decimal Amount { get; set; }
    public decimal DebitFee { get; set; }
    public decimal CreditFee { get; set; }
    
    public int? PaymentMethodId { get; set; }
    public PaymentMethodEntity PaymentMethod { get; set; }

    public PaymentEntity(
        int paymentId, 
        int commandId, 
        PaymentEnum method, 
        decimal amount,
        int paymentMethodId,
        decimal debitFee,
        decimal creditFee
    ) {
        PaymentId = paymentId;
        CommandId = commandId;
        Method = method;
        Amount = amount;
        PaymentMethodId = paymentMethodId;
        DebitFee = debitFee;
        CreditFee = creditFee;
    }

    public PaymentEntity()
    {
        
    }
}