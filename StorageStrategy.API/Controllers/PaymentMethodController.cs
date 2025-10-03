using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StorageStrategy.Domain.Commands.PaymentMethod;
using StorageStrategy.Domain.Repository;
using StorageStrategy.Models;
using StorageStrategy.Utils.Extensions;

namespace StorageStrategy.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Manager,Employee,Admin")]
public class PaymentMethodController : Controller
{
    
    [HttpGet("list")]
    public async Task<IActionResult> Get(
        [FromServices]IPaymentMethodRepository repo
    ) {
        var clients = await repo.GetAllAsync(p => p.CompanyId == User.GetCompanyId());
        return Ok(new Result(clients, "Busca realizada"));
    }
    
    [HttpPost("create")]
    public async Task<IActionResult> Create(
        [FromServices]IMediator _mediator,
        [FromBody] CreatePaymentMethodCommand command
    )
    {
        command.CompanyId = User.GetCompanyId();
        var response = await _mediator.Send(command);
        return Ok(response);
    }
    
    [HttpPut("update")]
    public async Task<IActionResult> Update(
        [FromServices]IMediator _mediator,
        [FromBody] UpdatePaymentMethodCommand command
    ) {
        command.CompanyId = User.GetCompanyId();
        var response = await _mediator.Send(command);
        return Ok(response);
    }
}