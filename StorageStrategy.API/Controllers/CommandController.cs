﻿using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StorageStrategy.Domain.Commands.Command;
using StorageStrategy.Domain.Repository;
using StorageStrategy.Models;
using StorageStrategy.Utils.Extensions;

namespace StorageStrategy.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Manager,Employee,Admin")]
    public class CommandController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public CommandController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet("list")]
        public async Task<IActionResult> ToList(
            [FromServices] ICommandRepository repo, 
            [FromQuery] int companyId,
            [FromQuery] bool haveEndDate
        ) {
            List<CreateCommandCommand> result = new();
            companyId = User.GetCompanyId();
            var commands = await repo.ToListAsync(companyId, haveEndDate);
            commands = commands.OrderByDescending(p => p.InitialDate).ToList();

            commands.ForEach(_command =>
            {
                CreateCommandCommand command = new CreateCommandCommand(_command);
                result.Add(command);
            });

            return Ok(new Result(result, "Busca realizada"));
        }

        [HttpGet("getcommand")]
        public async Task<IActionResult> GetCommand(
            [FromServices] ICommandRepository repo, 
            [FromQuery] int companyId,
            [FromQuery] int commandId
        ) {
            companyId = User.GetCompanyId();
            var entity = await repo.GetCommandByIdAsync(commandId, companyId);
            var command = new CreateCommandCommand(entity);
            return Ok(new Result(command, "Busca realizada"));
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreateCommandCommand command)
        {
            command.CompanyId = User.GetCompanyId();
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPost("finish-command")]
        public async Task<IActionResult> FinishCommand([FromBody] FinishCommandCommand command)
        {
            command.CompanyId = User.GetCompanyId();
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPut("add-product-command")]
        public async Task<IActionResult> AddProductCommand([FromBody] AddProductCommandCommand command)
        {
            command.CompanyId = User.GetCompanyId();
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        
        [HttpPut("remove-product-command/{commandId}/product/{productId}")]
        public async Task<IActionResult> RemoveProductCommand([FromRoute]int commandId, [FromRoute]int productId)
        {
            var result = await _mediator.Send(new RemoveProductCommandCommand(commandId, productId, User.GetCompanyId()));
            return Ok(result);
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] UpdateCommandCommand command)
        {
            command.CompanyId = User.GetCompanyId();
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> Delete([FromQuery]int commandId, [FromServices]ICommandRepository repo)
        {
            DeleteCommandCommand command = new(commandId, User.GetCompanyId());
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
