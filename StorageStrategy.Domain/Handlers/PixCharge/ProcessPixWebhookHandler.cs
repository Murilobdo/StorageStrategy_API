using MediatR;
using Microsoft.Extensions.Logging;
using StorageStrategy.Domain.Commands.PixCharge;
using StorageStrategy.Domain.Repository;
using StorageStrategy.Domain.Services.MercadoPago;
using StorageStrategy.Models;
using StorageStrategy.Utils.Services;

namespace StorageStrategy.Domain.Handlers.PixCharge;

public class ProcessPixWebhookHandler : HandlerBase, IRequestHandler<ProcessPixWebhookCommand, Result>
{
    private readonly IPixChargeRepository _repoPixCharge;
    private readonly IPixCredentialRepository _repoPixCredential;
    private readonly ICommandRepository _repoCommand;
    private readonly ISecretProtector _protector;
    private readonly IMercadoPagoGateway _gateway;
    private readonly ILogger<ProcessPixWebhookHandler> _logger;

    public ProcessPixWebhookHandler(
        IPixChargeRepository repoPixCharge,
        IPixCredentialRepository repoPixCredential,
        ICommandRepository repoCommand,
        ISecretProtector protector,
        IMercadoPagoGateway gateway,
        ILogger<ProcessPixWebhookHandler> logger
    )
    {
        _repoPixCharge = repoPixCharge;
        _repoPixCredential = repoPixCredential;
        _repoCommand = repoCommand;
        _protector = protector;
        _gateway = gateway;
        _logger = logger;
    }

    public async Task<Result> Handle(ProcessPixWebhookCommand request, CancellationToken cancellationToken)
    {
        var credential = await _repoPixCredential.GetByCompanyIdAsync(request.CompanyId);
        if (credential is null)
        {
            _logger.LogWarning("Webhook Mercado Pago recebido para empresa {CompanyId} sem credencial cadastrada", request.CompanyId);
            return CreateError("Credencial não encontrada para a empresa");
        }

        var pixCharge = await _repoPixCharge.GetByMpPaymentIdAsync(request.MpPaymentId);
        if (pixCharge is null)
        {
            _logger.LogWarning("Webhook Mercado Pago recebido para pagamento {MpPaymentId} sem PixCharge correspondente", request.MpPaymentId);
            return CreateError("Cobrança Pix não encontrada para este pagamento");
        }

        var accessToken = _protector.Unprotect(credential.AccessTokenEncrypted);
        var mpPayment = await _gateway.ConsultarPagamentoAsync(accessToken, request.MpPaymentId, cancellationToken);
        var newStatus = MercadoPagoStatusMapper.Map(mpPayment.Status);
        var previousStatus = pixCharge.Status;

        if (previousStatus == newStatus)
        {
            _logger.LogInformation(
                "Notificação de webhook ignorada (idempotente): PixCharge {PixChargeId}, status {Status} já aplicado",
                pixCharge.PixChargeId, newStatus);
            return CreateResponse(pixCharge, "Nenhuma alteração de status");
        }

        pixCharge.Status = newStatus;
        pixCharge.AtualizadoEm = DateTime.Now;
        _repoPixCharge.Update(pixCharge);
        await _repoPixCharge.SaveAsync();

        _logger.LogInformation(
            "PixCharge {PixChargeId} (tenant {CompanyId}, pedido {CommandId}, mp_payment_id {MpPaymentId}) mudou de {StatusAnterior} para {StatusNovo}",
            pixCharge.PixChargeId, pixCharge.CompanyId, pixCharge.CommandId, pixCharge.MpPaymentId, previousStatus, newStatus);

        if (newStatus == PixChargeStatusEnum.Pago)
            await AplicarPagamentoNaComandaAsync(pixCharge, cancellationToken);

        return CreateResponse(pixCharge, "Webhook processado com sucesso");
    }

    private async Task AplicarPagamentoNaComandaAsync(PixChargeEntity pixCharge, CancellationToken cancellationToken)
    {
        var command = await _repoCommand.GetCommandByIdAsync(pixCharge.CommandId, pixCharge.CompanyId);
        if (command is null)
        {
            _logger.LogWarning("PixCharge {PixChargeId} pago, mas a comanda {CommandId} não foi encontrada", pixCharge.PixChargeId, pixCharge.CommandId);
            return;
        }

        var payment = new PaymentEntity
        {
            CommandId = command.CommandId,
            Method = PaymentEnum.Pix,
            Amount = pixCharge.Valor,
            TotalFee = 0,
            AmountWithFee = pixCharge.Valor,
            PaymentMethodId = null
        };

        command.Payments.Add(payment);

        var totalPago = command.Payments.Sum(p => p.Amount);
        var totalComanda = command.TotalPrice - command.Discount + command.Increase;

        if (command.FinalDate is null && totalPago >= totalComanda)
            command.FinishCommand();

        _repoCommand.Update(command);
        await _repoCommand.SaveAsync();
    }
}
