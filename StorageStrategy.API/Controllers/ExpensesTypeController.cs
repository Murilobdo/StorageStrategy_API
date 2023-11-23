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
    [Authorize(Roles = "Manager,Employee,Admin")]
    public class ExpensesTypeController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public ExpensesTypeController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet("list")]
        public async Task<IActionResult> ToList([FromServices] IExpenseRepository repo, int companyId)
        {
          
            companyId = User.GetCompanyId();
            var expensesType = await repo.ReadExpensesTypeAsync(companyId);
            List<ExpensesTypeCommandBase> result = new();

            expensesType.ForEach(category =>
            {
                result.Add(_mapper.Map<ExpensesTypeCommandBase>(category));
            });

            return Ok(new Result(result, "Busca realizada"));
          
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody]CreateExpenseTypeCommand command)
        {
            command.CompanyId = User.GetCompanyId();
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPost("AddRangeExpensesType")]
        public async Task<IActionResult> AddRangeCategory(
            [FromServices] IExpenseRepository repo,
            [FromBody] List<CreateExpenseTypeCommand> commands)
        {
            var logs = new List<Error>();

            commands.ForEach(command => command.CompanyId = User.GetCompanyId());
            await repo.CreateTranscationAsync();

            foreach (var expensesType in commands)
            {
                var result = await _mediator.Send(expensesType);
                if(!result.Success)
                {
                    logs.AddRange(result.Errors);
                }
            }

            if(logs.Count == 0) {
                await repo.CommitAsync();
                return Ok(new Result(commands, $"{commands.Count} categorias importadas com sucesso"));
            } else {
                await repo.RollbackAsync();
                return Ok(new Result(logs, "Não foi possivel importar a planilha"));
            }
        }

        [HttpDelete("delete/{expenseTypeId:int}")]
        public async Task<IActionResult> Delete([FromRoute]int expenseTypeId)
        {
            var command = new DeleteExpenseTypeCommand(expenseTypeId, User.GetCompanyId());
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
