using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StorageStrategy.Domain.Commands.PixCharge;
using StorageStrategy.Domain.Repository;
using StorageStrategy.Domain.Services.MercadoPago;
using StorageStrategy.Utils;
using StorageStrategy.Utils.Services;

namespace StorageStrategy.API.Controllers;

public class MercadoPagoWebhookPayload
{
    public string? Type { get; set; }
    public string? Action { get; set; }
    public MercadoPagoWebhookData? Data { get; set; }
}

public class MercadoPagoWebhookData
{
    public string? Id { get; set; }
}

[ApiController]
[Route("api/webhooks/mercadopago")]
[AllowAnonymous]
public class MercadoPagoWebhookController : Controller
{
    private readonly IPixCredentialRepository _repoPixCredential;
    private readonly ISecretProtector _protector;
    private readonly IBackgroundTaskQueue _queue;
    private readonly ILogger<MercadoPagoWebhookController> _logger;

    public MercadoPagoWebhookController(
        IPixCredentialRepository repoPixCredential,
        ISecretProtector protector,
        IBackgroundTaskQueue queue,
        ILogger<MercadoPagoWebhookController> logger
    )
    {
        _repoPixCredential = repoPixCredential;
        _protector = protector;
        _queue = queue;
        _logger = logger;
    }

    [HttpPost("{companyId:int}")]
    public async Task<IActionResult> Receive([FromRoute] int companyId, [FromBody] MercadoPagoWebhookPayload payload)
    {
        var dataId = payload.Data?.Id ?? Request.Query["data.id"].ToString();
        if (string.IsNullOrWhiteSpace(dataId) || !long.TryParse(dataId, out var mpPaymentId))
            return BadRequest();

        var credential = await _repoPixCredential.GetByCompanyIdAsync(companyId);
        if (credential is null)
        {
            _logger.LogWarning("Webhook Mercado Pago recebido para empresa {CompanyId} sem credencial cadastrada", companyId);
            return Unauthorized();
        }

        var webhookSecret = _protector.Unprotect(credential.WebhookSecretEncrypted);
        var xSignature = Request.Headers["x-signature"].ToString();
        var xRequestId = Request.Headers["x-request-id"].ToString();

        if (!MercadoPagoSignatureValidator.IsValid(xSignature, xRequestId, dataId, webhookSecret))
        {
            _logger.LogWarning("Webhook Mercado Pago rejeitado por assinatura inválida (empresa {CompanyId})", companyId);
            return Unauthorized();
        }

        _queue.QueueWork(async (services, cancellationToken) =>
        {
            var mediator = services.GetRequiredService<IMediator>();
            await mediator.Send(new ProcessPixWebhookCommand(companyId, mpPaymentId), cancellationToken);
        });

        return Ok();
    }
}
