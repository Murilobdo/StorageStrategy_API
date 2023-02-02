using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StorageStrategy.Domain.Commands.Dashboard;

namespace StorageStrategy.API.Controllers
{
    [Route("dashboard")]
    [ApiController]
    public class DashboardController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public DashboardController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpPost("entry-and-exit-month")]
        public async Task<IActionResult> EntryAndExitOfMonth([FromBody] EntryAndExitOfMonthCommand command)
        {
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
