﻿using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using StorageStrategy.Domain.Commands.Category;
using StorageStrategy.Domain.Commands.Command;
using StorageStrategy.Domain.Commands.Products;
using StorageStrategy.Domain.Repository;
using StorageStrategy.Models;

namespace StorageStrategy.API.Controllers
{
    [ApiController]
    [Route("command")]
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
            try
            {
                var command = await repo.ToListAsync(companyId, haveEndDate);
                List<CreateCommandCommand> result = new();

                command.ForEach(category =>
                {
                    result.Add(_mapper.Map<CreateCommandCommand>(category));
                });

                return Ok(new Result(result, "Busca realizada"));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("getcommand")]
        public async Task<IActionResult> GetCommand(
            [FromServices] ICommandRepository repo, 
            [FromQuery] int companyId,
            [FromQuery] int commandId
        ) {
            try
            {
                var entity = await repo.GetCommandByIdAsync(commandId, companyId);
                var command = _mapper.Map<CreateCommandCommand>(entity);
                return Ok(new Result(command, "Busca realizada"));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreateCommandCommand command)
        {
            try
            {
                var result = await _mediator.Send(command);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("finish-command")]
        public async Task<IActionResult> FinishCommand([FromBody] FinishCommandCommand command)
        {
            try
            {
                var result = await _mediator.Send(command);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("add-product-command")]
        public async Task<IActionResult> AddProductCommand([FromBody] AddProductCommandCommand command)
        {
            try
            {
                var result = await _mediator.Send(command);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] UpdateCommandCommand command)
        {
            try
            {
                var result = await _mediator.Send(command);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("FinalizeCommand")]
        public async Task<IActionResult> FinalizeCommand([FromBody] DeleteCommandCommand command)
        {
            try
            {
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