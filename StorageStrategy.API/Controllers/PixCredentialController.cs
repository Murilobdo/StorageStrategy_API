using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StorageStrategy.Domain.Commands.PixCredential;
using StorageStrategy.Utils.Extensions;

namespace StorageStrategy.API.Controllers;

[ApiController]
[Route("api/pix-credentials")]
[Authorize(Roles = "Manager,Admin")]
public class PixCredentialController : Controller
{
    private readonly IMediator _mediator;

    public PixCredentialController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var response = await _mediator.Send(new GetPixCredentialStatusQuery(User.GetCompanyId()));
        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> Save([FromBody] SavePixCredentialCommand command)
    {
        command.CompanyId = User.GetCompanyId();
        command.EmployeeId = User.GetUserId();
        var response = await _mediator.Send(command);
        return Ok(response);
    }

    [HttpDelete]
    public async Task<IActionResult> Deactivate()
    {
        var response = await _mediator.Send(new DeactivatePixCredentialCommand(User.GetCompanyId()));
        return Ok(response);
    }
}
