using Microsoft.AspNetCore.DataProtection;
using StorageStrategy.Domain.Commands.PixCredential;
using StorageStrategy.Domain.Handlers.PixCredential;
using StorageStrategy.Tests.FakeRepository;
using StorageStrategy.Utils.Services;
using System.Threading;
using Xunit;

namespace StorageStrategy.Tests.Handler
{
    public class PixCredentialHandlerTest : HandlerBaseTest
    {
        private readonly FakePixCredentialRepository _repo;
        private readonly ISecretProtector _protector;
        private readonly CancellationToken _cancellationToken;

        public PixCredentialHandlerTest()
        {
            _repo = new FakePixCredentialRepository();
            _protector = new SecretProtector(new EphemeralDataProtectionProvider());
            _cancellationToken = new CancellationToken();
        }

        [Fact(DisplayName = "Sucesso ao Cadastrar Credenciais do Mercado Pago pela Primeira Vez")]
        public async Task Sucesso_ao_cadastrar_credenciais_pela_primeira_vez()
        {
            var handler = new SavePixCredentialHandler(_repo, _protector);
            var command = new SavePixCredentialCommand
            {
                CompanyId = 2,
                EmployeeId = 1,
                AccessToken = "APP_USR-novo-token-1234",
                WebhookSecret = "novo-segredo"
            };

            var result = await handler.Handle(command, _cancellationToken);
            var response = (PixCredentialStatusResponse)result.Response;

            Assert.True(result.Success);
            Assert.True(response.IsConfigured);
            Assert.Equal("1234", response.AccessTokenLastDigits);

            var salvo = _repo.credentials.First(p => p.CompanyId == 2);
            Assert.NotEqual("APP_USR-novo-token-1234", salvo.AccessTokenEncrypted);
            Assert.Equal("APP_USR-novo-token-1234", _protector.Unprotect(salvo.AccessTokenEncrypted));
        }

        [Fact(DisplayName = "Sucesso ao Atualizar Credenciais Existentes")]
        public async Task Sucesso_ao_atualizar_credenciais_existentes()
        {
            var handler = new SavePixCredentialHandler(_repo, _protector);
            var command = new SavePixCredentialCommand
            {
                CompanyId = 1,
                EmployeeId = 1,
                AccessToken = "APP_USR-token-atualizado",
                WebhookSecret = "segredo-atualizado"
            };

            var result = await handler.Handle(command, _cancellationToken);

            Assert.True(result.Success);
            Assert.Single(_repo.credentials.Where(p => p.CompanyId == 1));
        }

        [Fact(DisplayName = "Erro ao Cadastrar Credenciais Inválidas")]
        public async Task Erro_ao_cadastrar_credenciais_invalidas()
        {
            var handler = new SavePixCredentialHandler(_repo, _protector);
            var command = new SavePixCredentialCommand
            {
                CompanyId = 1,
                EmployeeId = 1,
                AccessToken = string.Empty,
                WebhookSecret = string.Empty
            };

            var result = await handler.Handle(command, _cancellationToken);

            Assert.False(result.Success);
        }

        [Fact(DisplayName = "Sucesso ao Consultar Status sem Nunca Retornar o Segredo em Texto Plano")]
        public async Task Sucesso_ao_consultar_status_sem_expor_segredo()
        {
            var handler = new GetPixCredentialStatusHandler(_repo);

            var result = await handler.Handle(new GetPixCredentialStatusQuery(1), _cancellationToken);
            var response = (PixCredentialStatusResponse)result.Response;

            Assert.True(response.IsConfigured);
            Assert.True(response.IsActive);
        }

        [Fact(DisplayName = "Sucesso ao Consultar Empresa sem Credencial Configurada")]
        public async Task Sucesso_ao_consultar_empresa_sem_credencial()
        {
            var handler = new GetPixCredentialStatusHandler(_repo);

            var result = await handler.Handle(new GetPixCredentialStatusQuery(999), _cancellationToken);
            var response = (PixCredentialStatusResponse)result.Response;

            Assert.False(response.IsConfigured);
        }

        [Fact(DisplayName = "Sucesso ao Desativar Credencial")]
        public async Task Sucesso_ao_desativar_credencial()
        {
            var handler = new DeactivatePixCredentialHandler(_repo);

            var result = await handler.Handle(new DeactivatePixCredentialCommand(1), _cancellationToken);

            Assert.True(result.Success);
            Assert.False(_repo.credentials.First(p => p.CompanyId == 1).IsActive);
        }

        [Fact(DisplayName = "Erro ao Desativar Credencial Inexistente")]
        public async Task Erro_ao_desativar_credencial_inexistente()
        {
            var handler = new DeactivatePixCredentialHandler(_repo);

            var result = await handler.Handle(new DeactivatePixCredentialCommand(999), _cancellationToken);

            Assert.False(result.Success);
        }
    }
}
