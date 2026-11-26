using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StorageStrategy.Domain.Commands.Print;
using StorageStrategy.Utils.Extensions;

namespace StorageStrategy.API.Controllers
{
    /// <summary>
    /// Impressão da comanda (feature 006) para funcionários (JWT). Endpoints do agente ficam no
    /// <see cref="PrintAgentController"/>, com outro esquema de autenticação.
    /// </summary>
    [ApiController]
    [Route("api/print")]
    [Authorize(Roles = "Manager,Employee,Admin")]
    public class PrintController : Controller
    {
        private readonly IMediator _mediator;

        public PrintController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public record CreatePrintJobRequest(int CommandId);

        [HttpPost("jobs")]
        public async Task<IActionResult> CreateJob([FromBody] CreatePrintJobRequest request)
        {
            var result = await _mediator.Send(new CreatePrintJobCommand
            {
                CommandId = request.CommandId,
                CompanyId = User.GetCompanyId(),
                EmployeeId = User.GetUserId()
            });
            return Ok(result);
        }

        [HttpGet("jobs/command/{commandId:int}/latest")]
        public async Task<IActionResult> GetLatest([FromRoute] int commandId)
        {
            var result = await _mediator.Send(new GetLatestPrintJobQuery(commandId, User.GetCompanyId()));
            return Ok(result);
        }

        [HttpPost("jobs/{printJobId:int}/cancel")]
        public async Task<IActionResult> Cancel([FromRoute] int printJobId)
        {
            var result = await _mediator.Send(new CancelPrintJobCommand(printJobId, User.GetCompanyId()));
            return Ok(result);
        }

        [HttpGet("station")]
        [Authorize(Roles = "Manager,Admin")]
        public async Task<IActionResult> GetStation()
        {
            var result = await _mediator.Send(new GetPrintStationQuery(User.GetCompanyId()));
            return Ok(result);
        }

        [HttpPut("station")]
        [Authorize(Roles = "Manager,Admin")]
        public async Task<IActionResult> SaveStation([FromBody] SavePrintStationCommand command)
        {
            command.CompanyId = User.GetCompanyId();
            command.EmployeeId = User.GetUserId();
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPost("station/regenerate-key")]
        [Authorize(Roles = "Manager,Admin")]
        public async Task<IActionResult> RegenerateKey()
        {
            var result = await _mediator.Send(new RegeneratePrintStationKeyCommand(User.GetCompanyId(), User.GetUserId()));
            return Ok(result);
        }
    }
}
