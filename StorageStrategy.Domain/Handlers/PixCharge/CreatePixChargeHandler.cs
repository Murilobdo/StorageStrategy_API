using MediatR;
using Microsoft.Extensions.Options;
using StorageStrategy.Domain.Commands.PixCharge;
using StorageStrategy.Domain.Repository;
using StorageStrategy.Domain.Services.MercadoPago;
using StorageStrategy.Models;
using StorageStrategy.Utils.Services;

namespace StorageStrategy.Domain.Handlers.PixCharge;

public class CreatePixChargeHandler : HandlerBase, IRequestHandler<CreatePixChargeCommand, Result>
{
    private readonly ICommandRepository _repoCommand;
    private readonly IEmployeeRepository _repoEmployee;
    private readonly IPixChargeRepository _repoPixCharge;
    private readonly IPixCredentialRepository _repoPixCredential;
    private readonly ISecretProtector _protector;
    private readonly IMercadoPagoGateway _gateway;
    private readonly IOptions<AppSettings> _appSettings;

    public CreatePixChargeHandler(
        ICommandRepository repoCommand,
        IEmployeeRepository repoEmployee,
        IPixChargeRepository repoPixCharge,
        IPixCredentialRepository repoPixCredential,
        ISecretProtector protector,
        IMercadoPagoGateway gateway,
        IOptions<AppSettings> appSettings
    )
    {
        _repoCommand = repoCommand;
        _repoEmployee = repoEmployee;
        _repoPixCharge = repoPixCharge;
        _repoPixCredential = repoPixCredential;
        _protector = protector;
        _gateway = gateway;
        _appSettings = appSettings;
    }

    public async Task<Result> Handle(CreatePixChargeCommand request, CancellationToken cancellationToken)
    {
        if (!request.IsValid())
            return CreateError(request.GetErros(), "Dados inválidos");

        var command = await _repoCommand.GetCommandByIdAsync(request.CommandId, request.CompanyId);
        if (command is null)
            return CreateError("Comanda não encontrada");

        var credential = await _repoPixCredential.GetActiveByCompanyIdAsync(request.CompanyId);
        if (credential is null)
            return CreateError("Configure suas credenciais do Mercado Pago antes de cobrar via Pix");

        var existingCharge = await _repoPixCharge.GetPendingByCommandIdAsync(request.CommandId, request.CompanyId);
        if (existingCharge is not null)
        {
            _repoPixCharge.Delete(existingCharge);
            await _repoPixCharge.SaveAsync();
        }

        var totalPago = command.Payments.Sum(p => p.Amount);
        var valorPendente = command.TotalPrice - command.Discount + command.Increase - totalPago;

        if (valorPendente <= 0)
            return CreateError("Esta comanda não possui saldo pendente para cobrança");

        var employee = await _repoEmployee.GetByIdAsync(command.EmployeeId, request.CompanyId);
        var payerEmail = employee?.Email;
        if (string.IsNullOrWhiteSpace(payerEmail))
            return CreateError("Não foi possível determinar um e-mail de pagador para a cobrança Pix");

        var accessToken = _protector.Unprotect(credential.AccessTokenEncrypted);
        var expiraEm = DateTime.Now.AddMinutes(_appSettings.Value.MercadoPago.DefaultExpirationMinutes);
        var idempotencyKey = $"command-{command.CommandId}-{Guid.NewGuid()}";

        // A cobrança só é persistida após a confirmação da API do Mercado Pago,
        // evitando registros "fantasma" no banco caso a requisição falhe.
        var mpResponse = await _gateway.CriarCobrancaPixAsync(
            accessToken,
            valorPendente,
            $"Comanda {command.Code}",
            payerEmail,
            expiraEm,
            idempotencyKey,
            cancellationToken
        );

        var pixCharge = new PixChargeEntity
        {
            CompanyId = request.CompanyId,
            CommandId = request.CommandId,
            Valor = valorPendente,
            Status = MercadoPagoStatusMapper.Map(mpResponse.Status),
            ExpiraEm = expiraEm,
            MpPaymentId = mpResponse.Id,
            QrCode = mpResponse.PointOfInteraction?.TransactionData?.QrCode,
            QrCodeBase64 = mpResponse.PointOfInteraction?.TransactionData?.QrCodeBase64,
            TicketUrl = mpResponse.PointOfInteraction?.TransactionData?.TicketUrl,
            AtualizadoEm = DateTime.Now
        };

        await _repoPixCharge.AddAsync(pixCharge);
        await _repoPixCharge.SaveAsync();

        return CreateResponse(pixCharge, "Cobrança Pix gerada com sucesso!");
    }
}
