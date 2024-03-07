using CleanArchitecture.Aggregation.Application.Interfaces.Repositories.Database;
using CleanArchitecture.Aggregation.Application.Interfaces.Repositories.RedisCache;
using FluentValidation;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArchitecture.Aggregation.Application.Features.Products.Commands.CreateProduct
{
    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        private readonly IProductRedisCacheAsync _productRedisCacheAsync;
        private readonly IProductRepositoryAsync productRepository;

        public CreateProductCommandValidator(
            IProductRepositoryAsync productRepository,
            IProductRedisCacheAsync productRedisCacheAsync
            )
        {
            this.productRepository = productRepository;
            _productRedisCacheAsync = productRedisCacheAsync;

            RuleFor(p => p.Barcode)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .MaximumLength(50).WithMessage("{PropertyName} must not exceed 50 characters.")
                .MustAsync(IsUniqueBarcode).WithMessage("{PropertyName} already exists.");

            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .MaximumLength(50).WithMessage("{PropertyName} must not exceed 50 characters.");

        }

        private async Task<bool> IsUniqueBarcode(string barcode, CancellationToken cancellationToken)
        {
            //return await productRepository.IsUniqueBarcodeAsync(barcode);
            return await _productRedisCacheAsync.IsUniqueBarcodeAsync(barcode);
        }
    }
}
