using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StorageStrategy.Domain.Commands.PixCharge;
using StorageStrategy.Utils.Extensions;

namespace StorageStrategy.API.Controllers;

[ApiController]
[Route("api/pagamentos/pix")]
[Authorize(Roles = "Manager,Employee,Admin")]
public class PixPaymentController : Controller
{
    private readonly IMediator _mediator;

    public PixPaymentController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreatePixChargeCommand command)
    {
        command.CompanyId = User.GetCompanyId();
        var response = await _mediator.Send(command);
        return Ok(response);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetStatus([FromRoute] int id)
    {
        var response = await _mediator.Send(new GetPixChargeStatusQuery(id, User.GetCompanyId()));
        return Ok(response);
    }
}
