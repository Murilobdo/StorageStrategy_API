using System.Text.Json.Serialization;

namespace StorageStrategy.Domain.Services.MercadoPago.Dto;

public class MercadoPagoCreatePaymentRequest
{
    [JsonPropertyName("transaction_amount")]
    public decimal TransactionAmount { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;

    [JsonPropertyName("payment_method_id")]
    public string PaymentMethodId { get; set; } = "pix";

    [JsonPropertyName("payer")]
    public MercadoPagoPayer Payer { get; set; } = new();

    [JsonPropertyName("date_of_expiration")]
    public string DateOfExpiration { get; set; } = string.Empty;
}

public class MercadoPagoPayer
{
    [JsonPropertyName("email")]
    public string Email { get; set; }
}

public class MercadoPagoPaymentResponse
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;

    [JsonPropertyName("status_detail")]
    public string? StatusDetail { get; set; }

    [JsonPropertyName("date_of_expiration")]
    public string? DateOfExpiration { get; set; }

    [JsonPropertyName("point_of_interaction")]
    public MercadoPagoPointOfInteraction? PointOfInteraction { get; set; }
}

public class MercadoPagoPointOfInteraction
{
    [JsonPropertyName("transaction_data")]
    public MercadoPagoTransactionData? TransactionData { get; set; }
}

public class MercadoPagoTransactionData
{
    [JsonPropertyName("qr_code")]
    public string? QrCode { get; set; }

    [JsonPropertyName("qr_code_base64")]
    public string? QrCodeBase64 { get; set; }

    [JsonPropertyName("ticket_url")]
    public string? TicketUrl { get; set; }
}
