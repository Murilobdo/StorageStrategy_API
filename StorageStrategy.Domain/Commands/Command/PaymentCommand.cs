using StorageStrategy.Models;

namespace StorageStrategy.Domain.Commands.Command;

public class PaymentCommand
{
    public int PaymentId { get; set; }
    public int PaymentMethodId { get; set; }
    public PaymentEnum Method { get; set; }
    public decimal Amount { get; set; }
    public decimal DebitFee { get; set; }
    public decimal CreditFee { get; set; }

    public PaymentCommand(int paymentId, PaymentEnum method, decimal amount)
    {
        PaymentId = paymentId;
        Method = method;
        Amount = amount;
    }

    public PaymentCommand()
    {
        
    }
}
