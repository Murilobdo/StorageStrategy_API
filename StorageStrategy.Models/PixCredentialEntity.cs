namespace StorageStrategy.Models;

public class PixCredentialEntity
{
    public int PixCredentialId { get; set; }
    public int CompanyId { get; set; }
    public string AccessTokenEncrypted { get; set; } = string.Empty;
    public string AccessTokenLastDigits { get; set; } = string.Empty;
    public string WebhookSecretEncrypted { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime CriadoEm { get; set; }
    public DateTime AtualizadoEm { get; set; }
    public int AtualizadoPorEmployeeId { get; set; }

    public PixCredentialEntity()
    {
        CriadoEm = DateTime.Now;
        AtualizadoEm = DateTime.Now;
    }
}
