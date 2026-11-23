namespace StorageStrategy.Domain.Commands.PixCredential;

public class PixCredentialStatusResponse
{
    public bool IsConfigured { get; set; }
    public bool IsActive { get; set; }
    public string? AccessTokenLastDigits { get; set; }
    public DateTime? AtualizadoEm { get; set; }
}
