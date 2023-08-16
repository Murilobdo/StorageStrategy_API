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
        public async Task<IActionResult> ToList([FromServices] IExpenseRepository repo, int companyId)
        {
            companyId = User.GetCompanyId();
            var expenses = await repo.ToList(companyId);
            List<ExpenseCommandBase> result = new();

            expenses.ForEach(category =>
            {
                result.Add(_mapper.Map<ExpenseCommandBase>(category));
            });

            return Ok(new Result(result, "Busca realizada"));
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody]CreateExpenseCommand command)
        {
            
            command.CompanyId = User.GetCompanyId();
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete("delete/{expenseId:int}")]
        public async Task<IActionResult> Delete([FromRoute]int expenseId)
        {
            var command = new DeleteExpenseCommand(expenseId, User.GetCompanyId());
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
