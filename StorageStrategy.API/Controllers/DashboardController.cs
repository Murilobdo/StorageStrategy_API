using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StorageStrategy.Domain.Commands.Dashboard;
using StorageStrategy.Utils.Extensions;

namespace StorageStrategy.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class DashboardController : ControllerBase
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
                command.CompanyId = User.GetCompanyId();
                var response = await _mediator.Send(command);
                return Ok(response);    
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("entry-and-exit-for-day")]
        public async Task<IActionResult> EntryAndExitForDay([FromBody] EntryAndExitForDayCommand command)
        {
            try
            {
                command.CompanyId = User.GetCompanyId();
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
