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
        public async Task<IActionResult> ToList([FromServices] IExpensesRepository repo, int companyId)
        {
            try
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
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody]CreateExpensesTypeCommand command)
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

        [HttpPost("AddRangeExpensesType")]
        public async Task<IActionResult> AddRangeCategory(
            [FromServices] IExpensesRepository repo,
            [FromBody] List<CreateExpensesTypeCommand> commands)
        {
            try
            {
                
                commands.ForEach(command => command.CompanyId = User.GetCompanyId());

                var logs = new List<Error>();
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
                }
                else {
                    await repo.RollbackAsync();
                    return Ok(new Result(logs, "Não foi possivel importar a planilha"));
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> Delete([FromBody] DeleteExpensesTypeCommand command)
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
