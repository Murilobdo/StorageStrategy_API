using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StorageStrategy.Domain.Repository;
using StorageStrategy.Models;
using StorageStrategy.Utils.Extensions;

namespace StorageStrategy.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Manager,Employee,Admin")]
public class ClientController : ControllerBase
{

    [HttpGet("get-clients/{companyId:int}")]
    public async Task<IActionResult> Get(
        [FromServices]IClientRepository repo,
        [FromRoute] int companyId
    )
    {
        var clients = await repo.GetClientsAsync(companyId);
        return Ok(new Result(clients, "Busca realizada"));
    }
    
    [HttpPost]
    public async Task<IActionResult> Post(
        [FromServices]IMediator mediator,
        [FromBody] CreateClientCommand command
    )
    {
        command.CompanyId = User.GetCompanyId();
        var result = await mediator.Send(command);
        return Ok(result);
    }
    
}