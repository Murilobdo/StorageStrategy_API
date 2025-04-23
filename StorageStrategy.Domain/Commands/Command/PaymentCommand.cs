using StorageStrategy.Models;

namespace StorageStrategy.Domain.Commands.Command;

public class PaymentCommand
{
    public int PaymentId { get; set; }
    public PaymentEnum Method { get; set; }
    public decimal Amount { get; set; }

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
