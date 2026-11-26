using Microsoft.AspNetCore.DataProtection;
using StorageStrategy.Utils.Services;
using Xunit;

namespace StorageStrategy.Tests.Utils
{
    public class SecretProtectorTest
    {
        private readonly ISecretProtector _protector;

        public SecretProtectorTest()
        {
            _protector = new SecretProtector(new EphemeralDataProtectionProvider());
        }

        [Fact]
        public void Sucesso_ao_cifrar_e_decifrar_um_segredo()
        {
            var original = "APP_USR-1234567890abcdef";

            var cifrado = _protector.Protect(original);
            var decifrado = _protector.Unprotect(cifrado);

            Assert.Equal(original, decifrado);
        }

        [Fact]
        public void Valor_cifrado_e_diferente_do_valor_original()
        {
            var original = "segredo-do-webhook";

            var cifrado = _protector.Protect(original);

            Assert.NotEqual(original, cifrado);
        }
    }
}
