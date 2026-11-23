using System.Net;
using Refit;
using StorageStrategy.Domain.Services.MercadoPago.Dto;
using StorageStrategy.Domain.Services.Refit;

namespace StorageStrategy.Domain.Services.MercadoPago;

public class MercadoPagoGateway : IMercadoPagoGateway
{
    private const int MaxAttempts = 3;

    private readonly IMercadoPagoService _service;

    public MercadoPagoGateway(IMercadoPagoService service)
    {
        _service = service;
    }

    public async Task<MercadoPagoPaymentResponse> CriarCobrancaPixAsync(
        string accessToken,
        decimal valor,
        string descricao,
        string emailPagador,
        DateTime expiraEm,
        string idempotencyKey,
        CancellationToken cancellationToken
    )
    {
        var request = new MercadoPagoCreatePaymentRequest
        {
            TransactionAmount = valor,
            Description = descricao,
            PaymentMethodId = "pix",
            Payer = new MercadoPagoPayer()
            {
                Email = emailPagador
            },
            DateOfExpiration = expiraEm.ToString("yyyy-MM-ddTHH:mm:ss.fffzzz")
        };

        return await ExecuteWithRetryAsync(
            () => _service.CreatePayment($"Bearer {accessToken}", idempotencyKey, request));
    }

    public async Task<MercadoPagoPaymentResponse> ConsultarPagamentoAsync(
        string accessToken,
        long mpPaymentId,
        CancellationToken cancellationToken
    ) {
        return await ExecuteWithRetryAsync(
            () => _service.GetPayment($"Bearer {accessToken}", mpPaymentId));
    }

    private static async Task<MercadoPagoPaymentResponse> ExecuteWithRetryAsync(Func<Task<MercadoPagoPaymentResponse>> action)
    {
        for (var attempt = 1; attempt <= MaxAttempts; attempt++)
        {
            try
            {
                return await action();
            }
            catch (ApiException ex) when (ex.StatusCode == (HttpStatusCode)429 && attempt < MaxAttempts)
            {
                var retryAfter = ex.Headers?.RetryAfter?.Delta ?? TimeSpan.FromSeconds(2 * attempt);
                await Task.Delay(retryAfter);
            }
        }

        return await action();
    }
}
