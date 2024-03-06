using CleanArchitecture.Aggregation.Application.Features.Products.Commands.CreateProduct;
using CleanArchitecture.Aggregation.Application.Features.Products.Commands.CreateProductRange;
using CleanArchitecture.Aggregation.Application.Features.Products.Commands.DeleteProductAll;
using CleanArchitecture.Aggregation.Application.Features.Products.Commands.DeleteProductById;
using CleanArchitecture.Aggregation.Application.Features.Products.Commands.UpdateProduct;
using CleanArchitecture.Aggregation.Application.Features.Products.Queries.GetAllProducts;
using CleanArchitecture.Aggregation.Application.Features.Products.Queries.GetProductById;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CleanArchitecture.Aggregation.WebApi.Controllers.v1
{
    [ApiVersion("1.0")]
    public class ProductController : BaseApiController
    {
        // GET: api/<controller>
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetAllProductsParameter filter)
        {

            return Ok(await Mediator.Send(new GetAllProductsQuery() { PageSize = filter.PageSize, PageNumber = filter.PageNumber }));
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await Mediator.Send(new GetProductByIdQuery { Id = id }));
        }

        // POST api/<controller>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Post(CreateProductCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        // POST api/<controller>/addrange
        [HttpPost("addrange")]
        [Authorize]
        public async Task<IActionResult> Post(CreateProductRangeCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Put(int id, UpdateProductCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }
            return Ok(await Mediator.Send(command));
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await Mediator.Send(new DeleteProductByIdCommand { Id = id }));
        }

        // DELETE api/<controller>/deleteall
        [HttpDelete("deleteall")]
        [Authorize]
        public async Task<IActionResult> DeleteAll()
        {
            return Ok(await Mediator.Send(new DeleteProductAllCommand()));
        }
    }
}
