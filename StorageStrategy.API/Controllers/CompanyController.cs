﻿using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using StorageStrategy.Domain.Commands.Category;
using StorageStrategy.Domain.Commands.Company;
using StorageStrategy.Domain.Repository;
using StorageStrategy.Models;

namespace StorageStrategy.API.Controllers
{
    [ApiController]
    [Route("company")]
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
            try
            {
                var companys = await repo.ToList();
                List<CompanyCommandBase> result = new();

                companys.ForEach(company =>
                {
                    result.Add(_mapper.Map<CompanyCommandBase>(company));
                });

                return Ok(new Result(result, "Busca realizada"));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
