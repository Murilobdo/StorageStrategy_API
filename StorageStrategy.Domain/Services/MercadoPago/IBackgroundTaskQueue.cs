namespace StorageStrategy.Domain.Services.MercadoPago;

public interface IBackgroundTaskQueue
{
    void QueueWork(Func<IServiceProvider, CancellationToken, Task> workItem);
    Task<Func<IServiceProvider, CancellationToken, Task>> DequeueAsync(CancellationToken cancellationToken);
}
