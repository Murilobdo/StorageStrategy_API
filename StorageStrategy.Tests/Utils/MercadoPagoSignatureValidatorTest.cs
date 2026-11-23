using System.Security.Cryptography;
using System.Text;
using StorageStrategy.Utils;
using Xunit;

namespace StorageStrategy.Tests.Utils
{
    public class MercadoPagoSignatureValidatorTest
    {
        private const string WebhookSecret = "segredo-do-webhook";

        private static string BuildValidSignatureHeader(string dataId, string requestId, string ts)
        {
            var manifest = $"id:{dataId};request-id:{requestId};ts:{ts};";
            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(WebhookSecret));
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(manifest));
            var v1 = Convert.ToHexString(hash).ToLowerInvariant();

            return $"ts={ts},v1={v1}";
        }

        [Fact]
        public void Sucesso_ao_validar_assinatura_correta()
        {
            var dataId = "999999999";
            var requestId = "req-123";
            var ts = "1704908010";

            var header = BuildValidSignatureHeader(dataId, requestId, ts);

            Assert.True(MercadoPagoSignatureValidator.IsValid(header, requestId, dataId, WebhookSecret));
        }

        [Fact]
        public void Erro_ao_validar_assinatura_com_segredo_diferente()
        {
            var dataId = "999999999";
            var requestId = "req-123";
            var ts = "1704908010";

            var header = BuildValidSignatureHeader(dataId, requestId, ts);

            Assert.False(MercadoPagoSignatureValidator.IsValid(header, requestId, dataId, "outro-segredo"));
        }

        [Fact]
        public void Erro_ao_validar_assinatura_ausente()
        {
            Assert.False(MercadoPagoSignatureValidator.IsValid(null, "req-123", "999999999", WebhookSecret));
        }

        [Fact]
        public void Erro_ao_validar_assinatura_com_request_id_alterado()
        {
            var dataId = "999999999";
            var ts = "1704908010";

            var header = BuildValidSignatureHeader(dataId, "req-123", ts);

            Assert.False(MercadoPagoSignatureValidator.IsValid(header, "req-outro", dataId, WebhookSecret));
        }
    }
}
