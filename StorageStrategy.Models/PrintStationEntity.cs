namespace StorageStrategy.Models;

/// <summary>
/// Estação de impressão da empresa: o computador do caixa com a impressora térmica ligada.
/// No MVP há uma estação (e uma impressora) ativa por empresa.
/// </summary>
public class PrintStationEntity
{
    public static readonly TimeSpan OnlineWindow = TimeSpan.FromSeconds(60);
    public const int DefaultPaperWidthMm = 58;

    public int PrintStationId { get; set; }
    public int CompanyId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int PaperWidthMm { get; set; } = DefaultPaperWidthMm;
    public PrintEncodingEnum Encoding { get; set; } = PrintEncodingEnum.Cp860;

    /// <summary>SHA-256 do segredo da chave da estação. Nunca sai em resposta.</summary>
    public byte[] KeyHash { get; set; } = Array.Empty<byte>();

    /// <summary>Últimos 4 caracteres da chave, só para identificação na tela.</summary>
    public string KeyLastChars { get; set; } = string.Empty;

    public bool IsActive { get; set; }
    public DateTime? LastSeenAt { get; set; }
    public DateTime CriadoEm { get; set; }
    public DateTime AtualizadoEm { get; set; }
    public int AtualizadoPorEmployeeId { get; set; }

    public PrintStationEntity()
    {
        CriadoEm = DateTime.Now;
        AtualizadoEm = DateTime.Now;
    }

    public bool IsOnline(DateTime now) =>
        LastSeenAt.HasValue && now - LastSeenAt.Value <= OnlineWindow;
}
