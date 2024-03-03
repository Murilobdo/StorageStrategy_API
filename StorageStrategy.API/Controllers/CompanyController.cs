using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StorageStrategy.Domain.Commands.Company;
using StorageStrategy.Domain.Commands.Login;
using StorageStrategy.Domain.Repository;
using StorageStrategy.Models;
using StorageStrategy.Utils.Extensions;

namespace StorageStrategy.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize]
    public class CompanyController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public CompanyController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromServices] ICompanyRepository repo)
        {
            var companys = await repo.ToList();
            List<CompanyCommandBase> result = new();

            companys.ForEach(company =>
            {
                result.Add(_mapper.Map<CompanyCommandBase>(company));
            });

            return Ok(new Result(result, "Busca realizada"));
        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetById([FromServices] ICompanyRepository repo)
        {
            var company = await repo.GetById(User.GetCompanyId());
            return Ok(new Result(company, "Busca realizada"));
        }

        [HttpPost("create")]
        public async Task<IActionResult> Post([FromBody] CreateCompanyCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }
    }
}
