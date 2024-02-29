using AutoMapper;
using CleanArchitecture.Aggregation.Application.Features.Products.Commands.CreateProduct;
using CleanArchitecture.Aggregation.Application.Interfaces.Repositories;
using CleanArchitecture.Aggregation.Domain.Entities;
using Moq;

namespace CleanArchitecture.Aggregation.Test.Mediator.Commands
{
    public class CreateProductCommandTest
    {
        // Write test method for CreateProductCommandHandler
        [Fact]
        public async Task CreateProductCommandHandler_ShouldReturnProduct()
        {
            // Arrange
            var product = new Product { Id = 1, Name = "Product 1", Barcode = "123", Rate = 4.5m };
            var productRepositoryAsyncMock = new Mock<IProductRepositoryAsync>();
            productRepositoryAsyncMock.Setup(x => x.AddAsync(It.IsAny<Product>())).ReturnsAsync(product);
            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(x => x.Map<Product>(It.IsAny<CreateProductCommand>())).Returns(product);
            var command = new CreateProductCommand { Name = "Product 1", Barcode = "123", Rate = 4.5m };
            var handler = new CreateProductCommandHandler(productRepositoryAsyncMock.Object, mapperMock.Object);

            // Act
            var result = await handler.Handle(command, new CancellationToken());

            // Assert
            Assert.Equal(product.Id, result.Data);
        }
    }
}
