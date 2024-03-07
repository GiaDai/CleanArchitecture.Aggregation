using CleanArchitecture.Aggregation.Application.Features.Products.Commands.CreateProduct;
using CleanArchitecture.Aggregation.Application.Features.Products.Commands.CreateProductRange;
using CleanArchitecture.Aggregation.Application.Features.Products.Commands.DeleteProductAll;
using CleanArchitecture.Aggregation.Application.Features.Products.Commands.DeleteProductById;
using CleanArchitecture.Aggregation.Application.Features.Products.Commands.UpdateProduct;
using CleanArchitecture.Aggregation.Application.Features.Products.Queries.GetAllProducts;
using CleanArchitecture.Aggregation.Application.Features.Products.Queries.GetProductById;
using CleanArchitecture.Aggregation.Application.Features.Products.Queries.SearchProductByName;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Text;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CleanArchitecture.Aggregation.WebApi.Controllers.v1
{
    //[Authorize]
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
        public async Task<IActionResult> Post(CreateProductCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        // POST api/<controller>/addrange
        [HttpPost("addrange")]
        public async Task<IActionResult> Post(CreateProductRangeCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
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
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await Mediator.Send(new DeleteProductByIdCommand { Id = id }));
        }

        // DELETE api/<controller>/deleteall
        [HttpDelete("deleteall")]
        public async Task<IActionResult> DeleteAll()
        {
            return Ok(await Mediator.Send(new DeleteProductAllCommand()));
        }

        // Method POST read file and check only accept .json file
        [HttpPost("upload")]
        public async Task<IActionResult> UploadJsonFile(IFormFile formFile)
        {
            // Check if file is null
            if (formFile == null)
            {
                return BadRequest("File is null");
            }
            // Check if file is not .json
            if (formFile.FileName.EndsWith(".json") == false)
            {
                return BadRequest("File is not .json");
            }
            // Read content of file and parse to list product not use Mediator
            var stringBuilder = new StringBuilder();
            using (var reader = new StreamReader(formFile.OpenReadStream()))
            {
                while (reader.Peek() >= 0)
                    stringBuilder.AppendLine(await reader.ReadLineAsync());
            }
            string strJson = stringBuilder.ToString();
            // try catch to parse json to list product
            try
            {
                var jsonObjects = JsonConvert.DeserializeObject<List<JObject>>(strJson);
                // check each item in the product list is a correct Product object, otherwise return a list of errors
                var validationResults = new List<ValidationResult>();
                var productValid = new List<CreateProductCommand>();
                var productNotValid = new List<JObject>();
                foreach (var jsonObject in jsonObjects)
                {
                    // Attempt to convert the JObject to a CreateProductCommand object
                    var product = jsonObject.ToObject<CreateProductCommand>();

                    if (product == null)
                    {
                        // If the conversion fails, return an error
                        productNotValid.Add(jsonObject);
                    }

                    // Validate the CreateProductCommand object
                    var context = new ValidationContext(product);
                    if (!Validator.TryValidateObject(product, context, validationResults, true))
                    {
                        // If the object is invalid, return the validation errors
                        productNotValid.Add(jsonObject);
                    }else
                    {
                        productValid.Add(product);
                    }
                }

                var result = await Mediator.Send(new CreateProductRangeCommand { Products = productValid });
                return Ok(new { Success = result.Data , InvalidProducts = productNotValid });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        // GET search product by name api/<controller>/search?name=abc
        [HttpGet("search")]
        public async Task<IActionResult> SearchProductByName([FromQuery] string name)
        {
            return Ok(await Mediator.Send(new SearchProductByNameQuery { SearchKey = name }));
        }
    }
}
