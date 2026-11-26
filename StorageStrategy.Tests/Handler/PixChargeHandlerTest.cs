using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using StorageStrategy.Domain.Commands.PixCharge;
using StorageStrategy.Domain.Handlers.PixCharge;
using StorageStrategy.Domain.Services.MercadoPago;
using StorageStrategy.Domain.Services.MercadoPago.Dto;
using StorageStrategy.Models;
using StorageStrategy.Tests.FakeRepository;
using StorageStrategy.Utils.Services;
using System.Threading;
using Xunit;

namespace StorageStrategy.Tests.Handler
{
    public class PixChargeHandlerTest : HandlerBaseTest
    {
        private readonly FakeCommandRepository _repoCommand;
        private readonly FakeEmployeeRepository _repoEmployee;
        private readonly FakePixChargeRepository _repoPixCharge;
        private readonly FakePixCredentialRepository _repoPixCredential;
        private readonly ISecretProtector _protector;
        private readonly IOptions<AppSettings> _appSettings;
        private readonly CancellationToken _cancellationToken;

        public PixChargeHandlerTest()
        {
            _repoCommand = new FakeCommandRepository();
            _repoEmployee = new FakeEmployeeRepository();
            _repoPixCharge = new FakePixChargeRepository();
            _repoPixCredential = new FakePixCredentialRepository();
            _protector = new SecretProtector(new EphemeralDataProtectionProvider());
            _appSettings = Options.Create(new AppSettings { MercadoPago = new MercadoPagoSettings { DefaultExpirationMinutes = 60 } });
            _cancellationToken = new CancellationToken();

            // A fake repository guarda placeholders; aqui substituímos por valores
            // realmente cifrados pelo mesmo protector usado nos handlers sob teste.
            var credential = _repoPixCredential.credentials[0];
            credential.AccessTokenEncrypted = _protector.Protect("APP_USR-fake-access-token");
            credential.WebhookSecretEncrypted = _protector.Protect("fake-webhook-secret");
        }

        private static Mock<IMercadoPagoGateway> BuildGatewayMock(long mpPaymentId = 123456, string status = "pending")
        {
            var mock = new Mock<IMercadoPagoGateway>();
            mock.Setup(g => g.CriarCobrancaPixAsync(
                    It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<string>(), It.IsAny<string>(),
                    It.IsAny<DateTime>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new MercadoPagoPaymentResponse
                {
                    Id = mpPaymentId,
                    Status = status,
                    PointOfInteraction = new MercadoPagoPointOfInteraction
                    {
                        TransactionData = new MercadoPagoTransactionData
                        {
                            QrCode = "00020126...",
                            QrCodeBase64 = "base64image",
                            TicketUrl = "https://mercadopago.com/ticket/123"
                        }
                    }
                });

            return mock;
        }

        private CreatePixChargeHandler BuildCreateHandler(Mock<IMercadoPagoGateway> gatewayMock)
        {
            return new CreatePixChargeHandler(
                _repoCommand, _repoEmployee, _repoPixCharge, _repoPixCredential,
                _protector, gatewayMock.Object, _appSettings);
        }

        #region CREATE PIX CHARGE

        [Fact(DisplayName = "Sucesso ao Criar Cobrança Pix para o Saldo Pendente da Comanda")]
        public async Task Sucesso_ao_criar_cobranca_pix()
        {
            var gatewayMock = BuildGatewayMock();
            var handler = BuildCreateHandler(gatewayMock);

            var command = new CreatePixChargeCommand { CommandId = 1, CompanyId = 1 };
            var result = await handler.Handle(command, _cancellationToken);
            var pixCharge = (PixChargeEntity)result.Response;

            Assert.True(result.Success);
            Assert.Equal(151, pixCharge.Valor);
            Assert.Equal(PixChargeStatusEnum.AguardandoPagamento, pixCharge.Status);
            Assert.Equal("00020126...", pixCharge.QrCode);
            gatewayMock.Verify(g => g.CriarCobrancaPixAsync(
                It.IsAny<string>(), 151, It.IsAny<string>(), It.IsAny<string>(),
                It.IsAny<DateTime>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact(DisplayName = "Erro ao Criar Cobrança Pix para Comanda Inexistente")]
        public async Task Erro_ao_criar_cobranca_pix_comanda_inexistente()
        {
            var handler = BuildCreateHandler(BuildGatewayMock());

            var command = new CreatePixChargeCommand { CommandId = 999, CompanyId = 1 };
            var result = await handler.Handle(command, _cancellationToken);

            Assert.False(result.Success);
        }

        [Fact(DisplayName = "Erro ao Criar Cobrança Pix sem Credenciais Configuradas")]
        public async Task Erro_ao_criar_cobranca_pix_sem_credenciais()
        {
            var gatewayMock = BuildGatewayMock();
            var handler = BuildCreateHandler(gatewayMock);

            // companyId 1 possui comanda cadastrada na FakeCommandRepository, mas a credencial fake só cobre a empresa 1;
            // aqui simulamos ausência de credencial desativando-a.
            _repoPixCredential.credentials[0].IsActive = false;

            var command = new CreatePixChargeCommand { CommandId = 1, CompanyId = 1 };
            var result = await handler.Handle(command, _cancellationToken);

            Assert.False(result.Success);
            gatewayMock.Verify(g => g.CriarCobrancaPixAsync(
                It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<string>(), It.IsAny<string>(),
                It.IsAny<DateTime>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact(DisplayName = "Erro ao Criar Cobrança Pix para Comanda sem Saldo Pendente")]
        public async Task Erro_ao_criar_cobranca_pix_sem_saldo_pendente()
        {
            var gatewayMock = BuildGatewayMock();
            var handler = BuildCreateHandler(gatewayMock);

            var command = _repoCommand.commands.First(p => p.CommandId == 1);
            command.Payments.Add(new PaymentEntity(2, 1, PaymentEnum.Cash, 151, 1, 0));

            var result = await handler.Handle(new CreatePixChargeCommand { CommandId = 1, CompanyId = 1 }, _cancellationToken);

            Assert.False(result.Success);
        }

        [Fact(DisplayName = "Sucesso ao Reaproveitar Cobrança Pix Pendente (Idempotência de Negócio)")]
        public async Task Sucesso_ao_reaproveitar_cobranca_pendente()
        {
            var gatewayMock = BuildGatewayMock();
            var handler = BuildCreateHandler(gatewayMock);

            var existente = new PixChargeEntity
            {
                PixChargeId = 1,
                CompanyId = 1,
                CommandId = 1,
                Valor = 151,
                Status = PixChargeStatusEnum.AguardandoPagamento,
                ExpiraEm = DateTime.Now.AddHours(1)
            };
            _repoPixCharge.charges.Add(existente);

            var result = await handler.Handle(new CreatePixChargeCommand { CommandId = 1, CompanyId = 1 }, _cancellationToken);
            var pixCharge = (PixChargeEntity)result.Response;

            Assert.True(result.Success);
            Assert.Equal(existente.PixChargeId, pixCharge.PixChargeId);
            gatewayMock.Verify(g => g.CriarCobrancaPixAsync(
                It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<string>(), It.IsAny<string>(),
                It.IsAny<DateTime>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        #endregion

        #region GET STATUS

        [Fact(DisplayName = "Sucesso ao Consultar Status de uma Cobrança Pix")]
        public async Task Sucesso_ao_consultar_status()
        {
            _repoPixCharge.charges.Add(new PixChargeEntity { PixChargeId = 5, CompanyId = 1, CommandId = 1, Status = PixChargeStatusEnum.AguardandoPagamento });

            var handler = new GetPixChargeStatusHandler(_repoPixCharge);
            var result = await handler.Handle(new GetPixChargeStatusQuery(5, 1), _cancellationToken);

            Assert.True(result.Success);
        }

        [Fact(DisplayName = "Erro ao Consultar Cobrança Pix de Outra Empresa (Isolamento Multi-Tenant)")]
        public async Task Erro_ao_consultar_cobranca_de_outra_empresa()
        {
            _repoPixCharge.charges.Add(new PixChargeEntity { PixChargeId = 5, CompanyId = 1, CommandId = 1, Status = PixChargeStatusEnum.AguardandoPagamento });

            var handler = new GetPixChargeStatusHandler(_repoPixCharge);
            var result = await handler.Handle(new GetPixChargeStatusQuery(5, 2), _cancellationToken);

            Assert.False(result.Success);
        }

        #endregion

        #region WEBHOOK

        private ProcessPixWebhookHandler BuildWebhookHandler(Mock<IMercadoPagoGateway> gatewayMock)
        {
            return new ProcessPixWebhookHandler(
                _repoPixCharge, _repoPixCredential, _repoCommand, _protector, gatewayMock.Object,
                new Mock<ILogger<ProcessPixWebhookHandler>>().Object);
        }

        [Fact(DisplayName = "Sucesso ao Processar Webhook de Pagamento Aprovado e Aplicar na Comanda")]
        public async Task Sucesso_ao_processar_webhook_aprovado()
        {
            var pixCharge = new PixChargeEntity
            {
                PixChargeId = 10,
                CompanyId = 1,
                CommandId = 1,
                MpPaymentId = 555,
                Valor = 151,
                Status = PixChargeStatusEnum.AguardandoPagamento
            };
            _repoPixCharge.charges.Add(pixCharge);

            var gatewayMock = BuildGatewayMock(555, "approved");
            gatewayMock.Setup(g => g.ConsultarPagamentoAsync(It.IsAny<string>(), 555, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new MercadoPagoPaymentResponse { Id = 555, Status = "approved" });

            var handler = BuildWebhookHandler(gatewayMock);
            var result = await handler.Handle(new ProcessPixWebhookCommand(1, 555), _cancellationToken);

            Assert.True(result.Success);
            Assert.Equal(PixChargeStatusEnum.Pago, pixCharge.Status);

            var command = _repoCommand.commands.First(p => p.CommandId == 1);
            Assert.Contains(command.Payments, p => p.Method == PaymentEnum.Pix && p.Amount == 151);
        }

        [Fact(DisplayName = "Sucesso ao Ignorar Webhook Duplicado (Idempotência de Processamento)")]
        public async Task Sucesso_ao_ignorar_webhook_duplicado()
        {
            var pixCharge = new PixChargeEntity
            {
                PixChargeId = 11,
                CompanyId = 1,
                CommandId = 1,
                MpPaymentId = 556,
                Valor = 151,
                Status = PixChargeStatusEnum.Pago
            };
            _repoPixCharge.charges.Add(pixCharge);

            var gatewayMock = BuildGatewayMock(556, "approved");
            gatewayMock.Setup(g => g.ConsultarPagamentoAsync(It.IsAny<string>(), 556, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new MercadoPagoPaymentResponse { Id = 556, Status = "approved" });

            var handler = BuildWebhookHandler(gatewayMock);
            await handler.Handle(new ProcessPixWebhookCommand(1, 556), _cancellationToken);

            var command = _repoCommand.commands.First(p => p.CommandId == 1);
            var pagamentosPix = command.Payments.Count(p => p.Method == PaymentEnum.Pix && p.Amount == 151);

            Assert.Equal(0, pagamentosPix);
        }

        [Fact(DisplayName = "Erro ao Processar Webhook para Empresa sem Credencial")]
        public async Task Erro_ao_processar_webhook_sem_credencial()
        {
            var gatewayMock = BuildGatewayMock();
            var handler = BuildWebhookHandler(gatewayMock);

            var result = await handler.Handle(new ProcessPixWebhookCommand(999, 1), _cancellationToken);

            Assert.False(result.Success);
        }

        [Fact(DisplayName = "Erro ao Processar Webhook para Pagamento sem PixCharge Correspondente")]
        public async Task Erro_ao_processar_webhook_sem_pixcharge()
        {
            var gatewayMock = BuildGatewayMock();
            var handler = BuildWebhookHandler(gatewayMock);

            var result = await handler.Handle(new ProcessPixWebhookCommand(1, 999999), _cancellationToken);

            Assert.False(result.Success);
        }

        #endregion
    }
}
