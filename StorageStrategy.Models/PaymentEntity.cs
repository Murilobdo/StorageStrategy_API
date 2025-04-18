namespace StorageStrategy.Models;

public class PaymentEntity
{
    public int PaymentId { get; set; }
    public int CommandId { get; set; }
    public CommandEntity Command { get; set; } = new();
    public PaymentEnum Method { get; set; }
    public decimal Amount { get; set; }

    public PaymentEntity(int paymentId, int commandId, PaymentEnum method, decimal amount)
    {
        PaymentId = paymentId;
        CommandId = commandId;
        Method = method;
        Amount = amount;
    }
}