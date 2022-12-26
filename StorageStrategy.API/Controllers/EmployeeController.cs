using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using StorageStrategy.Domain.Commands.Category;
using StorageStrategy.Domain.Commands.Employee;
using StorageStrategy.Domain.Commands.Products;
using StorageStrategy.Domain.Repository;

namespace StorageStrategy.API.Controllers
{
    [ApiController]
    [Route("employee")]
    public class EmployeeController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public EmployeeController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet("list")]
        public async Task<IActionResult> ToList([FromServices] IEmployeeRepository repo, int companyId)
        {
            try
            {
                var categorys = await repo.ToList(companyId);
                List<CreateEmployeeCommand> result = new();

                categorys.ForEach(category =>
                {
                    result.Add(_mapper.Map<CreateEmployeeCommand>(category));
                });

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreateEmployeeCommand command)
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
        public async Task<IActionResult> Update([FromBody] UpdateEmployeeCommand command)
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
        public async Task<IActionResult> Delete([FromBody] DeleteEmployeeCommand command)
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
