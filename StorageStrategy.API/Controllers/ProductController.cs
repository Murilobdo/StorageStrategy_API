using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using StorageStrategy.Domain.Commands.Category;
using StorageStrategy.Domain.Commands.Products;
using StorageStrategy.Domain.Repository;

namespace StorageStrategy.API.Controllers
{
    [ApiController]
    [Route("product")]
    public class ProductController : Controller
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
                var categorys = await repo.ToList(companyId);
                List<CreateProductCommand> result = new();

                categorys.ForEach(category =>
                {
                    result.Add(_mapper.Map<CreateProductCommand>(category));
                });

                return Ok(result);
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
                var result = await _mediator.Send(command);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> Delete([FromBody] DeleteProductCommand command)
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
