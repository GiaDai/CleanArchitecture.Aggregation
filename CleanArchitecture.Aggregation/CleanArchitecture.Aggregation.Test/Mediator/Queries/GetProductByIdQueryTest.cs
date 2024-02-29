using AutoMapper;
using CleanArchitecture.Aggregation.Application.Features.Products.Queries.GetProductById;
using CleanArchitecture.Aggregation.Application.Interfaces.Repositories;
using CleanArchitecture.Aggregation.Domain.Entities;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Aggregation.Test.Mediator.Queries
{
    public class GetProductByIdQueryTest
    {
        // Write test method for GetProductByIdQueryHandler
        [Fact]
        public async Task GetProductByIdQueryHandler_ShouldReturnProduct()
        {
            // Arrange
            var product = new Product { Id = 1, Name = "Product 1", Barcode = "123", Rate = 4.5m };
            var productRepositoryAsyncMock = new Mock<IProductRepositoryAsync>();
            productRepositoryAsyncMock.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(product);
            var mapperMock = new Mock<IMapper>();
            var query = new GetProductByIdQuery { Id = 1 };
            var handler = new GetProductByIdQuery.GetProductByIdQueryHandler(productRepositoryAsyncMock.Object, mapperMock.Object);

            // Act
            var result = await handler.Handle(query, new CancellationToken());

            // Assert
            Assert.Equal(product.Id, result.Data.Id);
            Assert.Equal(product.Name, result.Data.Name);
            Assert.Equal(product.Barcode, result.Data.Barcode);
            Assert.Equal(product.Rate, result.Data.Rate);
        }
    }
}
