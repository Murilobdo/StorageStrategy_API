using Microsoft.AspNetCore.DataProtection;

namespace StorageStrategy.Utils.Services
{
    public interface ISecretProtector
    {
        string Protect(string plainText);
        string Unprotect(string cipherText);
    }

    public class SecretProtector : ISecretProtector
    {
        private readonly IDataProtector _protector;

        public SecretProtector(IDataProtectionProvider provider)
        {
            _protector = provider.CreateProtector("PixCredential.v1");
        }

        public string Protect(string plainText) => _protector.Protect(plainText);
        public string Unprotect(string cipherText) => _protector.Unprotect(cipherText);
    }
}
