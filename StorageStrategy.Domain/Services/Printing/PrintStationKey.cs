using System.Security.Cryptography;
using System.Text;

namespace StorageStrategy.Domain.Services.Printing;

/// <summary>
/// Chave da estação de impressão: <c>ps_{PrintStationId}_{segredo}</c>, com segredo de 32 bytes aleatórios
/// em base64url. No banco fica só o SHA-256 do segredo (alta entropia: hash lento não é necessário,
/// ver research R5). A chave completa só existe na resposta de criação/regeneração.
/// </summary>
public static class PrintStationKey
{
    public const string Prefix = "ps_";
    private const int SecretBytes = 32;

    public static string Generate(int printStationId, out byte[] secretHash)
    {
        var secret = Base64Url(RandomNumberGenerator.GetBytes(SecretBytes));
        secretHash = Hash(secret);
        return $"{Prefix}{printStationId}_{secret}";
    }

    public static byte[] Hash(string secret) => SHA256.HashData(Encoding.UTF8.GetBytes(secret));

    public static bool TryParse(string? key, out int printStationId, out string secret)
    {
        printStationId = 0;
        secret = string.Empty;

        if (string.IsNullOrWhiteSpace(key) || !key.StartsWith(Prefix, StringComparison.Ordinal))
            return false;

        var rest = key[Prefix.Length..];
        var separator = rest.IndexOf('_');
        if (separator <= 0 || separator == rest.Length - 1)
            return false;

        if (!int.TryParse(rest[..separator], out printStationId) || printStationId <= 0)
            return false;

        secret = rest[(separator + 1)..];
        return true;
    }

    public static bool Verify(string secret, byte[] expectedHash)
    {
        if (expectedHash is null || expectedHash.Length == 0)
            return false;

        return CryptographicOperations.FixedTimeEquals(Hash(secret), expectedHash);
    }

    public static string LastChars(string key) => key.Length >= 4 ? key[^4..] : key;

    private static string Base64Url(byte[] bytes) =>
        Convert.ToBase64String(bytes).TrimEnd('=').Replace('+', '-').Replace('/', '_');
}
