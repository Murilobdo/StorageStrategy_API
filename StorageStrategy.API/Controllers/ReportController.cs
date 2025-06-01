using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StorageStrategy.Domain.Commands.Report;
using StorageStrategy.Domain.Repository;
using StorageStrategy.Utils.Extensions;

namespace StorageStrategy.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Manager,Admin")]
    public class ReportController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public ReportController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpPost("GetCommands")]
        public async Task<IActionResult> GetCommands(
            [FromServices] IReportRepository repo, 
            [FromBody] ReadCommandsBetweenDatesCommand command
        ) {
            command.CompanyId = User.GetCompanyId();
            var response = await _mediator.Send(command);
            return Ok(response);
        }
        
        [HttpPost("GetCommandsByMounth")]
        public async Task<IActionResult> GetCommandsByMounth(
            [FromServices] IReportRepository repo, 
            [FromBody] ReadCommandsByMounthCommand command
        ) {
            command.CompanyId = User.GetCompanyId();
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [HttpPost("GetPaymentsOfCommands")]
        public async Task<IActionResult> GetPaymentsOfCommands(
            [FromServices] IReportRepository repo,
            [FromBody] ReadPaymentCommandCommand command
        ) {
            command.CompanyId = User.GetCompanyId();
            var response = await _mediator.Send(command);
            return Ok(response);
        }
    }
}