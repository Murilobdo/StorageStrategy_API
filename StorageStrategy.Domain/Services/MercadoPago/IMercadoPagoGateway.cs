using StorageStrategy.Domain.Services.MercadoPago.Dto;

namespace StorageStrategy.Domain.Services.MercadoPago;

public interface IMercadoPagoGateway
{
    Task<MercadoPagoPaymentResponse> CriarCobrancaPixAsync(
        string accessToken,
        decimal valor,
        string descricao,
        string emailPagador,
        DateTime expiraEm,
        string idempotencyKey,
        CancellationToken cancellationToken
    );

    Task<MercadoPagoPaymentResponse> ConsultarPagamentoAsync(
        string accessToken,
        long mpPaymentId,
        CancellationToken cancellationToken
    );
}
