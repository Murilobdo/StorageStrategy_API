using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using StorageStrategy.Domain.Repository;
using StorageStrategy.Models;

namespace StorageStrategy.Domain.Services.MercadoPago;

public class PixExpirationJob : BackgroundService
{
    private static readonly TimeSpan Interval = TimeSpan.FromMinutes(5);

    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<PixExpirationJob> _logger;

    public PixExpirationJob(IServiceScopeFactory scopeFactory, ILogger<PixExpirationJob> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var timer = new PeriodicTimer(Interval);

        do
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();
                var repo = scope.ServiceProvider.GetRequiredService<IPixChargeRepository>();

                var expiredCharges = await repo.GetExpiredPendingAsync();
                foreach (var charge in expiredCharges)
                {
                    charge.Status = PixChargeStatusEnum.Expirado;
                    charge.AtualizadoEm = DateTime.Now;
                    repo.Update(charge);
                }

                if (expiredCharges.Count > 0)
                    await repo.SaveAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Falha ao processar expiração de cobranças Pix");
            }
        }
        while (!stoppingToken.IsCancellationRequested && await timer.WaitForNextTickAsync(stoppingToken));
    }
}
