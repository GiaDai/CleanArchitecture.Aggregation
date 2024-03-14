using CleanArchitecture.Aggregation.Application.Exceptions;
using CleanArchitecture.Aggregation.Application.Interfaces.Repositories.Database;
using CleanArchitecture.Aggregation.Application.Interfaces.Repositories.Elastic;
using CleanArchitecture.Aggregation.Application.Interfaces.Repositories.RedisCache;
using CleanArchitecture.Aggregation.Application.Wrappers;
using CleanArchitecture.Aggregation.Domain.Entities;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArchitecture.Aggregation.Application.Features.Products.Commands.UpdateProduct
{
    public class UpdateProductCommand : IRequest<Response<Product>>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Rate { get; set; }
        public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Response<Product>>
        {
            private readonly IProductElasticAsync _productElastic;
            private readonly IProductRedisCacheAsync _productRedisCache;
            private readonly IProductRepositoryAsync _productRepository;
            public UpdateProductCommandHandler(
                IProductRepositoryAsync productRepository,
                IProductRedisCacheAsync productRedisCache,
                IProductElasticAsync productElastic
                )
            {
                _productElastic = productElastic;
                _productRedisCache = productRedisCache;
                _productRepository = productRepository;
            }
            public async Task<Response<Product>> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
            {
                var product = await _productRepository.GetByIdAsync(command.Id);

                if (product == null)
                {
                    throw new ApiException($"Product Not Found.");
                }
                else
                {
                    product.Name = command.Name;
                    product.Rate = command.Rate;
                    product.Description = command.Description;
                    await _productRepository.UpdateAsync(product);
                    await _productElastic.RemoveProductAsync(product.Id.ToString(), "product");
                    await _productElastic.AddProductAsync(product, "product");
                    await _productRedisCache.RemoveAsync(product.Barcode);
                    await _productRedisCache.AddAsync(product.Barcode, product, TimeSpan.FromDays(1));
                    return new Response<Product>(product);
                }
            }
        }
    }
}
