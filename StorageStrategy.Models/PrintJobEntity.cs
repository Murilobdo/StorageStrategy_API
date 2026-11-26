namespace StorageStrategy.Models;

/// <summary>
/// Uma solicitação de impressão de uma comanda (fila de impressão por empresa).
/// </summary>
public class PrintJobEntity
{
    public const int FailureReasonMaxLength = 200;

    public int PrintJobId { get; set; }
    public int CompanyId { get; set; }
    public int CommandId { get; set; }
    public int RequestedByEmployeeId { get; set; }
    public int? PrintStationId { get; set; }

    /// <summary>Número da via: 1 + quantidade de jobs Impresso da comanda.</summary>
    public int Copy { get; set; } = 1;

    /// <summary>Bytes ESC/POS prontos. Nunca logar.</summary>
    public byte[] Payload { get; set; } = Array.Empty<byte>();

    public PrintJobStatusEnum Status { get; set; } = PrintJobStatusEnum.Aguardando;
    public string? FailureReason { get; set; }

    /// <summary>Gerado a cada reserva (claim); o resultado só é aceito com o mesmo valor.</summary>
    public Guid? ClaimId { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime? ClaimedAt { get; set; }
    public DateTime? CompletedAt { get; set; }

    public bool IsFinal =>
        Status is PrintJobStatusEnum.Impresso
            or PrintJobStatusEnum.Falhou
            or PrintJobStatusEnum.Cancelado
            or PrintJobStatusEnum.Expirado;
}
