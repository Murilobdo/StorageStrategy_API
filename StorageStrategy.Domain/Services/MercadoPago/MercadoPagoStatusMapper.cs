using StorageStrategy.Models;

namespace StorageStrategy.Domain.Services.MercadoPago;

public static class MercadoPagoStatusMapper
{
    public static PixChargeStatusEnum Map(string mpStatus)
    {
        return mpStatus?.ToLowerInvariant() switch
        {
            "pending" => PixChargeStatusEnum.AguardandoPagamento,
            "approved" => PixChargeStatusEnum.Pago,
            "rejected" => PixChargeStatusEnum.Rejeitado,
            "cancelled" => PixChargeStatusEnum.Cancelado,
            "refunded" => PixChargeStatusEnum.Rejeitado,
            _ => PixChargeStatusEnum.AguardandoPagamento
        };
    }
}
