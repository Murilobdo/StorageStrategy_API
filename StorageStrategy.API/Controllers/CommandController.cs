using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using StorageStrategy.Domain.Commands.Category;
using StorageStrategy.Domain.Commands.Command;
using StorageStrategy.Domain.Commands.Products;
using StorageStrategy.Domain.Repository;

namespace StorageStrategy.API.Controllers
{
    [ApiController]
    [Route("product")]
    public class CommandController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public CommandController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet("list")]
        public async Task<IActionResult> ToList([FromServices] ICommandRepository repo, int companyId)
        {
            try
            {
                var categorys = await repo.ToList(companyId);
                List<CreateCommandCommand> result = new();

                categorys.ForEach(category =>
                {
                    result.Add(_mapper.Map<CreateCommandCommand>(category));
                });

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreateCommandCommand command)
        {
            try
            {
                var result = await _mediator.Send(command);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] UpdateCommandCommand command)
        {
            try
            {
                var result = await _mediator.Send(command);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> Delete([FromBody] DeleteCommandCommand command)
        {
            try
            {
                var result = await _mediator.Send(command);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
