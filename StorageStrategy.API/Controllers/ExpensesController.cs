using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StorageStrategy.Domain.Commands.Expenses;
using StorageStrategy.Domain.Repository;
using StorageStrategy.Models;
using StorageStrategy.Utils.Extensions;

namespace StorageStrategy.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ExpensesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public ExpensesController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet("list")]
        public async Task<IActionResult> ToList([FromServices] IExpensesRepository repo, int companyId)
        {
            try
            {
                companyId = User.GetCompanyId();
                var expenses = await repo.ToList(companyId);
                List<ExpensesCommandBase> result = new();

                expenses.ForEach(category =>
                {
                    result.Add(_mapper.Map<ExpensesCommandBase>(category));
                });

                return Ok(new Result(result, "Busca realizada"));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody]CreateExpensesCommand command)
        {
            try
            {
                command.CompanyId = User.GetCompanyId();
                var result = await _mediator.Send(command);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> Delete([FromBody] DeleteExpensesCommand command)
        {
            try
            {
                command.CompanyId = User.GetCompanyId();
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
