using CleanArchitecture.Aggregation.Application.Interfaces.Repositories;
using CleanArchitecture.Aggregation.Domain.Entities;
using CleanArchitecture.Aggregation.Infrastructure.Persistence.Contexts;
using CleanArchitecture.Aggregation.Infrastructure.Persistence.Repositories;
using CleanArchitecture.Aggregation.Test.TestProvider;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Aggregation.Test.Repository
{
    public class ProductRepositoryAsyncTest
    {
        private Mock<ApplicationDbContext> _dbContext = new Mock<ApplicationDbContext>();
        private ProductRepositoryAsync _productRepositoryAsync;

        public ProductRepositoryAsyncTest()
        {
            _productRepositoryAsync = new ProductRepositoryAsync(_dbContext.Object);
        }

        // Test for IsUniqueBarcodeAsync method
        [Fact]
        public async Task IsUniqueBarcodeAsync_ShouldReturnTrue()
        {
            // Arrange
           var products = new TestAsyncEnumerable<Product>(new List<Product>
           {
                new Product { Id = 1, Name = "Product 1", Barcode = "123456", Description = "Description 1", Rate = 10 },
                new Product { Id = 2, Name = "Product 2", Barcode = "123457", Description = "Description 2", Rate = 20 }
            }).AsQueryable();

            var mockSet = new Mock<DbSet<Product>>();

            mockSet.As<IAsyncEnumerable<Product>>().Setup(m => m.GetAsyncEnumerator(new CancellationToken()))
                .Returns(new TestAsyncEnumerator<Product>(products.GetEnumerator()));
            mockSet.As<IQueryable<Product>>().Setup(m => m.Provider).Returns(new TestAsyncQueryProvider<Product>(products.Provider));
            mockSet.As<IQueryable<Product>>().Setup(m => m.Expression).Returns(products.Expression);
            mockSet.As<IQueryable<Product>>().Setup(m => m.ElementType).Returns(products.ElementType);
            mockSet.As<IQueryable<Product>>().Setup(m => m.GetEnumerator()).Returns(products.GetEnumerator());
            _dbContext.Setup(x => x.Products).Returns(mockSet.Object);
            // Act
            var result = await _productRepositoryAsync.IsUniqueBarcodeAsync("123456");

            // Assert
            Assert.True(result);
        }
    }
}
