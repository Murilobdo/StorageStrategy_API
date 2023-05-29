using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using StorageStrategy.Domain.Commands.Report;
using StorageStrategy.Domain.Repository;

namespace StorageStrategy.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
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
            [FromBody] ReadCommandsByMounthCommand command
        ) {
            try
            {
                var response = await _mediator.Send(command);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
      
    }
}