﻿using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StorageStrategy.Domain.Commands.Category;
using StorageStrategy.Domain.Repository;
using StorageStrategy.Models;
using StorageStrategy.Utils.Extensions;

namespace StorageStrategy.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CategoryController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public CategoryController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet("list")]
        public async Task<IActionResult> ToList([FromServices] ICategoryRepository repo, int companyId)
        {
            try
            {
                companyId = User.GetCompanyId();
                var categorys = await repo.ToList(companyId);
                List<CreateCategoryCommand> result = new();

                categorys.ForEach(category =>
                {
                    result.Add(_mapper.Map<CreateCategoryCommand>(category));
                });

                return Ok(new Result(result, "Busca realizada"));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreateCategoryCommand command)
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

        [HttpPost("AddRangeCategory")]
        public async Task<IActionResult> AddRangeCategory(
            [FromServices] ICategoryRepository repo,
            [FromBody] List<CreateCategoryCommand> command)
        {
            try
            {
                command.ForEach(c => c.CompanyId = User.GetCompanyId());

                var logs = new List<Error>();
                await repo.CreateTranscationAsync();

                foreach (var category in command)
                {
                    var result = await _mediator.Send(category);
                    if (!result.Success)
                        logs.AddRange(result.Errors);
                }

                if (logs.Count == 0)
                {
                    await repo.CommitAsync();
                    return Ok(new Result(command, $"{command.Count} categorias importadas com sucesso"));
                }
                else
                {
                    await repo.RollbackAsync();
                    return Ok(new Result(logs, "Não foi possivel importar a planilha"));
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] UpdateCategoryCommand command)
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

        [HttpDelete("delete")]
        public async Task<IActionResult> Delete([FromBody] DeleteCategoryCommand command)
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
