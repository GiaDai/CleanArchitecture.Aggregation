using CleanArchitecture.Aggregation.Application.Features.Products.Commands.CreateProduct;
using CleanArchitecture.Aggregation.Application.Interfaces.Repositories;
using CleanArchitecture.Aggregation.Domain.Entities;
using Moq;

namespace CleanArchitecture.Aggregation.Test.Validator
{
    public class CreateProductCommandValidatorTest
    {
        // Write test method for CreateProductCommandValidator with barcode is required
        [Theory]
        [InlineData(null)]
        public async Task CreateProductCommandValidator_WithBarcodeIsRequired_ShouldReturnFalse(string barcode)
        {
            // Arrange
            var productRepositoryAsyncMock = new Mock<IProductRepositoryAsync>();
            var validator = new CreateProductCommandValidator(productRepositoryAsyncMock.Object);
            var command = new CreateProductCommand { Name = "Product 1", Barcode = barcode, Rate = 4.5m };

            // Act
            var result = await validator.ValidateAsync(command);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == nameof(command.Barcode));
        }

        // Write test method for CreateProductCommandValidator with barcode is not exists has fake product return true
        [Theory]
        [InlineData("789")]
        public async Task CreateProductCommandValidator_WithBarcodeIsNotExists_ShouldReturnTrue(string barcode)
        {
            // Generate fake list of products
            var fakeProducts = new List<Product>
            {
                new Product { Id = 1, Name = "Product 1", Barcode = "123", Rate = 4.5m },
                new Product { Id = 2, Name = "Product 2", Barcode = "456", Rate = 4.5m }
            };
            // Arrange
            var productRepositoryAsyncMock = new Mock<IProductRepositoryAsync>();
            productRepositoryAsyncMock.Setup(x => x.IsUniqueBarcodeAsync(It.IsAny<string>()))
                .ReturnsAsync((string barcode) => !fakeProducts.Any(p => p.Barcode == barcode));
            var validator = new CreateProductCommandValidator(productRepositoryAsyncMock.Object);
            var command = new CreateProductCommand { Name = "Product 1", Barcode = barcode, Rate = 4.5m };

            // Act
            var result = await validator.ValidateAsync(command);

            // Assert
            Assert.True(result.IsValid);
        }

        // Write test method for CreateProductCommandValidator with barcode already exists
        [Theory]
        [InlineData("123")]
        public async Task CreateProductCommandValidator_WithBarcodeAlreadyExists_ShouldReturnFalse(string barcode)
        {
            // Arrange
            // generate list of products
            var fakeProducts = new List<Product>
            {
                new Product { Id = 1, Name = "Product 1", Barcode = "123", Rate = 4.5m },
                new Product { Id = 2, Name = "Product 2", Barcode = "456", Rate = 4.5m }
            };
            var productRepositoryAsyncMock = new Mock<IProductRepositoryAsync>();
            productRepositoryAsyncMock.Setup(x => x.IsUniqueBarcodeAsync(It.IsAny<string>()))
                .ReturnsAsync((string barcode) => !fakeProducts.Any(p => p.Barcode == barcode));
            var validator = new CreateProductCommandValidator(productRepositoryAsyncMock.Object);
            var command = new CreateProductCommand { Name = "Product 1", Barcode = barcode, Rate = 4.5m };

            // Act
            var result = await validator.ValidateAsync(command);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == nameof(command.Barcode));
        }

        // Write test method for CreateProductCommandValidator with barcode must not exceed 50 characters
        [Fact]
        public async Task CreateProductCommandValidator_WithBarcodeMustNotExceed50Characters_ShouldReturnFalse()
        {
            // Arrange
            var productRepositoryAsyncMock = new Mock<IProductRepositoryAsync>();
            productRepositoryAsyncMock.Setup(x => x.IsUniqueBarcodeAsync(It.IsAny<string>())).ReturnsAsync(true);
            var validator = new CreateProductCommandValidator(productRepositoryAsyncMock.Object);
            // Generate barcode with 51 characters
            var command = new CreateProductCommand { Name = "Product 1", Barcode = new string('A', 51), Rate = 4.5m };

            // Act
            var result = await validator.ValidateAsync(command);

            // Assert
            Assert.False(result.IsValid);
        }

        // Write test method for CreateProductCommandValidator with invalid name
        [Fact]
        public async Task CreateProductCommandValidator_WithInvalidName_ShouldReturnFalse()
        {
            // Arrange
            var product = new Product { Id = 1, Name = "Product 1", Barcode = "123", Rate = 4.5m };
            var productRepositoryAsyncMock = new Mock<IProductRepositoryAsync>();
            productRepositoryAsyncMock.Setup(x => x.IsUniqueBarcodeAsync("123")).ReturnsAsync(true);
            var validator = new CreateProductCommandValidator(productRepositoryAsyncMock.Object);
            var command = new CreateProductCommand { Name = "", Barcode = "123", Rate = 4.5m };

            // Act
            var result = await validator.ValidateAsync(command);

            // Assert
            Assert.False(result.IsValid);
        }

        // Write test method for CreateProductCommandValidator with name must not exceed 50 characters
        [Fact]
        public async Task CreateProductCommandValidator_WithNameMustNotExceed50Characters_ShouldReturnFalse()
        {
            // Arrange
            var product = new Product { Id = 1, Name = "Product 1", Barcode = "123", Rate = 4.5m };
            var productRepositoryAsyncMock = new Mock<IProductRepositoryAsync>();
            productRepositoryAsyncMock.Setup(x => x.IsUniqueBarcodeAsync("123")).ReturnsAsync(true);
            var validator = new CreateProductCommandValidator(productRepositoryAsyncMock.Object);
            // Generate name with 51 characters
            var command = new CreateProductCommand { Name = new string('A', 51), Barcode = "123", Rate = 4.5m };

            // Act
            var result = await validator.ValidateAsync(command);

            Assert.False(result.IsValid);
        }
    }
}
