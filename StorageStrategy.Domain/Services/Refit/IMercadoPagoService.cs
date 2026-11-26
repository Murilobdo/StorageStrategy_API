using Refit;
using StorageStrategy.Domain.Services.MercadoPago.Dto;

namespace StorageStrategy.Domain.Services.Refit
{
    public interface IMercadoPagoService
    {
        [Post("/v1/payments")]
        Task<MercadoPagoPaymentResponse> CreatePayment(
            [Header("Authorization")] string bearerToken,
            [Header("X-Idempotency-Key")] string idempotencyKey,
            [Body] MercadoPagoCreatePaymentRequest request
        );

        [Get("/v1/payments/{id}")]
        Task<MercadoPagoPaymentResponse> GetPayment(
            [Header("Authorization")] string bearerToken,
            long id
        );
    }
}
