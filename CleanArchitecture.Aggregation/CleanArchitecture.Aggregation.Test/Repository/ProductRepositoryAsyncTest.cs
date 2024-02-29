using CleanArchitecture.Aggregation.Application.Interfaces;
using CleanArchitecture.Aggregation.Domain.Entities;
using CleanArchitecture.Aggregation.Infrastructure.Persistence.Contexts;
using CleanArchitecture.Aggregation.Infrastructure.Persistence.Repositories;
using CleanArchitecture.Aggregation.Test.TestProvider;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace CleanArchitecture.Aggregation.Test.Repository
{
    public class ProductRepositoryAsyncTest
    {
        // Test for IsUniqueBarcodeAsync method use InMemoryDatabase should return false
        [Fact]
        public async Task IsUniqueBarcodeAsync_ShouldReturnFalse()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "IsUniqueBarcodeAsync_ShouldReturnFalse")
                .Options;
            var dateTimeServiceMock = new Mock<IDateTimeService>();
            var authenticatedUserServiceMock = new Mock<IAuthenticatedUserService>();

            var _mockDbContext = new ApplicationDbContext(options, dateTimeServiceMock.Object, authenticatedUserServiceMock.Object);
            var _productRepositoryAsync = new ProductRepositoryAsync(_mockDbContext);

            _mockDbContext.Products.Add(new Product { Id = 1, Name = "Product 1", Barcode = "123" });
            _mockDbContext.Products.Add(new Product { Id = 2, Name = "Product 2", Barcode = "456" });
            await _mockDbContext.SaveChangesAsync();

            // Act
            var result = await _productRepositoryAsync.IsUniqueBarcodeAsync("123");

            // Assert
            Assert.False(result);
        }

        // Test for IsUniqueBarcodeAsync method use InMemoryDatabase should return true
        [Fact]
        public async Task IsUniqueBarcodeAsync_ShouldReturnTrue()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "IsUniqueBarcodeAsync_ShouldReturnTrue")
                .Options;
            var dateTimeServiceMock = new Mock<IDateTimeService>();
            var authenticatedUserServiceMock = new Mock<IAuthenticatedUserService>();

            var _mockDbContext = new ApplicationDbContext(options, dateTimeServiceMock.Object, authenticatedUserServiceMock.Object);
            var _productRepositoryAsync = new ProductRepositoryAsync(_mockDbContext);

            _mockDbContext.Products.Add(new Product { Id = 1, Name = "Product 1", Barcode = "123" });
            _mockDbContext.Products.Add(new Product { Id = 2, Name = "Product 2", Barcode = "456" });
            await _mockDbContext.SaveChangesAsync();

            // Act
            var result = await _productRepositoryAsync.IsUniqueBarcodeAsync("789");

            // Assert
            Assert.True(result);
        }

        // Test for ComputeAverageRateAsync method use InMemoryDatabase should return 3.5
        [Fact]
        public async Task ComputeAverageRateAsync_ShouldReturn3_5()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "ComputeAverageRateAsync_ShouldReturn3_5")
                .Options;
            var dateTimeServiceMock = new Mock<IDateTimeService>();
            var authenticatedUserServiceMock = new Mock<IAuthenticatedUserService>();

            var _mockDbContext = new ApplicationDbContext(options, dateTimeServiceMock.Object, authenticatedUserServiceMock.Object);
            var _productRepositoryAsync = new ProductRepositoryAsync(_mockDbContext);

            _mockDbContext.Products.Add(new Product { Id = 1, Name = "Product 1", Rate = 3 });
            _mockDbContext.Products.Add(new Product { Id = 2, Name = "Product 2", Rate = 4 });
            await _mockDbContext.SaveChangesAsync();

            // Act
            var result = await _productRepositoryAsync.ComputeAverageRateAsync();

            // Assert
            Assert.Equal(3.5, result);
        }
    }
}
