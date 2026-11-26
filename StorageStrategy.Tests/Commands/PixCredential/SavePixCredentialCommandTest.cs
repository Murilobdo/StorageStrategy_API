using StorageStrategy.Domain.Commands.PixCredential;
using Xunit;

namespace StorageStrategy.Tests.Commands.PixCredential
{
    public class SavePixCredentialCommandTest : CommandBaseTest
    {
        [Fact]
        public void Sucesso_ao_validar_credenciais_do_mercado_pago()
        {
            SavePixCredentialCommand command = new()
            {
                CompanyId = 1,
                AccessToken = "APP_USR-1234567890",
                WebhookSecret = "segredo-do-webhook"
            };

            Assert.True(command.IsValid());
        }

        [Fact]
        public void Erro_ao_salvar_credencial_sem_empresa()
        {
            SavePixCredentialCommand command = new()
            {
                CompanyId = 0,
                AccessToken = "APP_USR-1234567890",
                WebhookSecret = "segredo-do-webhook"
            };

            Assert.True(MensagemDeErroExistente(command.GetErros(), "O Id da empresa é obrigatório"));
        }

        [Fact]
        public void Erro_ao_salvar_credencial_sem_access_token()
        {
            SavePixCredentialCommand command = new()
            {
                CompanyId = 1,
                AccessToken = string.Empty,
                WebhookSecret = "segredo-do-webhook"
            };

            Assert.Contains(command.GetErros(), e => e.ErrorMessage == "O Access Token do Mercado Pago é obrigatório");
        }

        [Fact]
        public void Erro_ao_salvar_credencial_com_access_token_em_formato_invalido()
        {
            SavePixCredentialCommand command = new()
            {
                CompanyId = 1,
                AccessToken = "token-qualquer",
                WebhookSecret = "segredo-do-webhook"
            };

            Assert.True(MensagemDeErroExistente(command.GetErros(), "O Access Token informado não parece um token válido do Mercado Pago"));
        }

        [Fact]
        public void Erro_ao_salvar_credencial_sem_webhook_secret()
        {
            SavePixCredentialCommand command = new()
            {
                CompanyId = 1,
                AccessToken = "APP_USR-1234567890",
                WebhookSecret = string.Empty
            };

            Assert.True(MensagemDeErroExistente(command.GetErros(), "O segredo do webhook é obrigatório"));
        }
    }
}
