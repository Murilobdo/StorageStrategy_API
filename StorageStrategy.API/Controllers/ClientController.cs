using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StorageStrategy.Domain.Commands;
using StorageStrategy.Domain.Commands.Client;
using StorageStrategy.Domain.Repository;
using StorageStrategy.Models;
using StorageStrategy.Utils.Extensions;

namespace StorageStrategy.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Manager,Employee,Admin")]
public class ClientController : ControllerBase
{
    private readonly IMediator _mediator;
    public ClientController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("get-clients")]
    public async Task<IActionResult> Get([FromServices]IClientRepository repo)
    {
        var clients = await repo.GetClientsAsync(User.GetCompanyId());
        return Ok(new Result(clients, "Busca realizada"));
    }
    
    [HttpGet("get-clients-with-commands")]
    public async Task<IActionResult> GetWithCommands()
    {
        var response = await _mediator.Send(new GetClientsTotalCommandQuery(User.GetCompanyId()));
        return Ok(response);
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
    
    [HttpPut]
    public async Task<IActionResult> Put([FromBody] UpdateClientCommand command) 
    {
        command.CompanyId = User.GetCompanyId();
        var result = await _mediator.Send(command);
        return Ok(result);
    }
}