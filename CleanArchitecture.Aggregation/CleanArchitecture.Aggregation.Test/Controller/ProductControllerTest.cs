using CleanArchitecture.Aggregation.Application.Features.Products.Queries.GetProductById;
using CleanArchitecture.Aggregation.Application.Wrappers;
using CleanArchitecture.Aggregation.Domain.Entities;
using CleanArchitecture.Aggregation.WebApi.Controllers.v1;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace CleanArchitecture.Aggregation.Test.Controller
{
    public class ProductControllerTest
    {
        // Write test Action Get for ProductController with GetAllProductsParameter params return OkObjectResult
        [Fact]
        public async Task Get_WithValidParameters_ShouldReturnOk()
        {
            // Arrange
            var mockMediator = new Mock<IMediator>();
            var product = new Product { 
                /* Initialize product here */ 
                Id = 1,
                Name = "Product 1",
                Description = "Description 1",
                Rate = 5,
                Barcode = "123456"
            };
            var expectedProduct = new Response<Product>(product);

            mockMediator.Setup(x => x.Send(It.IsAny<GetProductByIdQuery>(), default)).ReturnsAsync(expectedProduct);

            // Mock HttpContext and RequestServices
            var mockHttpContext = new Mock<HttpContext>();
            var mockServiceProvider = new Mock<IServiceProvider>();

            var controller = new ProductController();
            mockServiceProvider.Setup(x => x.GetService(typeof(IMediator))).Returns(mockMediator.Object);
            mockHttpContext.Setup(x => x.RequestServices).Returns(mockServiceProvider.Object);

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = mockHttpContext.Object
            };
            // Act
            var result = await controller.Get(1);

            // Assert
            var okeResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<Response<Product>>(okeResult.Value);
            Assert.True(response.Succeeded);
            Assert.Equal("Product 1", response.Data.Name);
            Assert.Equal("Description 1", response.Data.Description);
            Assert.Equal(5, response.Data.Rate);
            Assert.Equal("123456", response.Data.Barcode);
        }

        // Write test Action Get by Id for ProductController with id return Response<Product> Succeeded is false, Data is null and Message is "Product Not Found"
        [Fact]
        public async Task Get_WithInvalidId_ShouldReturnNotFound()
        {
            // Arrange
            var mockMediator = new Mock<IMediator>();
            var expectedProduct = new Response<Product>("Product Not Found");

            mockMediator.Setup(x => x.Send(It.IsAny<GetProductByIdQuery>(), default)).ReturnsAsync(expectedProduct);

            // Mock HttpContext and RequestServices
            var mockHttpContext = new Mock<HttpContext>();
            var mockServiceProvider = new Mock<IServiceProvider>();

            var controller = new ProductController();
            mockServiceProvider.Setup(x => x.GetService(typeof(IMediator))).Returns(mockMediator.Object);
            mockHttpContext.Setup(x => x.RequestServices).Returns(mockServiceProvider.Object);

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = mockHttpContext.Object
            };
            // Act
            var result = await controller.Get(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<Response<Product>>(okResult.Value);
            Assert.False(response.Succeeded);
            Assert.Null(response.Data);
            Assert.Equal("Product Not Found", response.Message);
        }

        // Write test Action Get by Id for ProductController with id response timeout return StatusCode 408
        [Fact]
        public async Task Get_WithTimeout_ShouldReturnStatusCode408()
        {
            // Arrange
            var mockMediator = new Mock<IMediator>();
            var expectedProduct = new Response<Product>("Request Timeout");

            mockMediator.Setup(x => x.Send(It.IsAny<GetProductByIdQuery>(), default)).ReturnsAsync(expectedProduct);

            // Mock HttpContext and RequestServices
            var mockHttpContext = new Mock<HttpContext>();
            var mockServiceProvider = new Mock<IServiceProvider>();

            var controller = new ProductController();
            mockServiceProvider.Setup(x => x.GetService(typeof(IMediator))).Returns(mockMediator.Object);
            mockHttpContext.Setup(x => x.RequestServices).Returns(mockServiceProvider.Object);

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = mockHttpContext.Object
            };
            // Act
            var result = await controller.Get(1);

            // Assert
            var statusCodeResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(StatusCodes.Status408RequestTimeout, statusCodeResult.StatusCode);
        }
    }


}
