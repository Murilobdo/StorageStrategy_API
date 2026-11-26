using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StorageStrategy.API.Configuration;
using StorageStrategy.Domain.Commands.Print;
using StorageStrategy.Utils.Extensions;

namespace StorageStrategy.API.Controllers
{
    /// <summary>
    /// Endpoints do agente de impressão (StorageStrategy.PrintAgent), autenticado com a chave da estação.
    /// A empresa e a estação vêm sempre das claims da chave, nunca do corpo.
    /// </summary>
    [ApiController]
    [Route("api/print/agent")]
    [Authorize(AuthenticationSchemes = PrintStationAuthenticationHandler.SchemeName, Roles = PrintStationAuthenticationHandler.RoleName)]
    public class PrintAgentController : Controller
    {
        private readonly IMediator _mediator;

        public PrintAgentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public record PrintResultRequest(Guid ClaimId, bool Success, string? FailureReason);

        [HttpPost("claim")]
        public async Task<IActionResult> Claim()
        {
            var result = await _mediator.Send(new ClaimPrintJobCommand(User.GetCompanyId(), StationId()));
            if (!result.Success)
                return Unauthorized();

            return result.Response is ClaimedPrintJobResponse job ? Ok(job) : NoContent();
        }

        /// <summary>Confirma a chave e devolve a empresa (usado pelo instalador do agente).</summary>
        [HttpGet("ping")]
        public async Task<IActionResult> Ping()
        {
            var result = await _mediator.Send(new PingPrintStationQuery(User.GetCompanyId(), StationId()));
            return result.Success ? Ok(result.Response) : Unauthorized();
        }

        [HttpPost("jobs/{printJobId:int}/result")]
        public async Task<IActionResult> Result([FromRoute] int printJobId, [FromBody] PrintResultRequest request)
        {
            var result = await _mediator.Send(new ReportPrintResultCommand
            {
                PrintJobId = printJobId,
                CompanyId = User.GetCompanyId(),
                ClaimId = request.ClaimId,
                Success = request.Success,
                FailureReason = request.FailureReason
            });

            return result.Response switch
            {
                PrintResultOutcome.Accepted => Ok(result),
                PrintResultOutcome.Conflict => Conflict(result),
                _ => NotFound(result)
            };
        }

        private int StationId() =>
            int.Parse(User.FindFirst(PrintStationAuthenticationHandler.StationIdClaim)!.Value);
    }
}
