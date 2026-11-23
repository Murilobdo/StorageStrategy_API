using System.Security.Cryptography;
using System.Text;

namespace StorageStrategy.Utils
{
    public static class MercadoPagoSignatureValidator
    {
        public static bool IsValid(string? xSignatureHeader, string? xRequestId, string dataId, string webhookSecret)
        {
            if (string.IsNullOrWhiteSpace(xSignatureHeader) || string.IsNullOrWhiteSpace(xRequestId) || string.IsNullOrWhiteSpace(dataId))
                return false;

            var parts = xSignatureHeader
                .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Select(part => part.Split('=', 2))
                .Where(kv => kv.Length == 2)
                .ToDictionary(kv => kv[0].Trim(), kv => kv[1].Trim());

            if (!parts.TryGetValue("ts", out var ts) || !parts.TryGetValue("v1", out var v1))
                return false;

            var manifest = $"id:{dataId};request-id:{xRequestId};ts:{ts};";

            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(webhookSecret));
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(manifest));
            var computedSignature = Convert.ToHexString(hash).ToLowerInvariant();

            return CryptographicOperations.FixedTimeEquals(
                Encoding.UTF8.GetBytes(computedSignature),
                Encoding.UTF8.GetBytes(v1.ToLowerInvariant()));
        }
    }
}
