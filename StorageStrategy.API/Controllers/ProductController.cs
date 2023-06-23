using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StorageStrategy.Domain.Commands.Products;
using StorageStrategy.Domain.Repository;
using StorageStrategy.Models;
using StorageStrategy.Utils.Extensions;

namespace StorageStrategy.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProductController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public ProductController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet("list")]
        public async Task<IActionResult> ToList([FromServices] IProductRepository repo, int companyId)
        {
            try
            {
                companyId = User.GetCompanyId();
                var products = await repo.ToList(companyId);
                List<CreateProductCommand> listProduct = new();

                products.ForEach(category =>
                {
                    listProduct.Add(_mapper.Map<CreateProductCommand>(category));
                });


                return Ok(new Result(listProduct, "Busca realizada"));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreateProductCommand command)
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

        [HttpPost("ImportExcel")]
        public async Task<IActionResult> ImportExcel(
            [FromBody] ImportProductCommand command)
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

        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] UpdateProductCommand command)
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

        [HttpDelete("delete/{productId:int}")]
        public async Task<IActionResult> Delete([FromRoute] int productId)
        {
            try
            {
                DeleteProductCommand command = new(productId, User.GetCompanyId());
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
