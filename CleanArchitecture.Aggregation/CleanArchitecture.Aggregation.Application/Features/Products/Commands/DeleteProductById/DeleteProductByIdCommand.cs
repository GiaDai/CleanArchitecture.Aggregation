using CleanArchitecture.Aggregation.Application.Exceptions;
using CleanArchitecture.Aggregation.Application.Interfaces.Repositories.Database;
using CleanArchitecture.Aggregation.Application.Interfaces.Repositories.Elastic;
using CleanArchitecture.Aggregation.Application.Interfaces.Repositories.RedisCache;
using CleanArchitecture.Aggregation.Application.Wrappers;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArchitecture.Aggregation.Application.Features.Products.Commands.DeleteProductById
{
    public class DeleteProductByIdCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
        public class DeleteProductByIdCommandHandler : IRequestHandler<DeleteProductByIdCommand, Response<int>>
        {
            private readonly IProductRedisCacheAsync _productRedisCache;
            private readonly IProductElasticAsync _productElastic;
            private readonly IProductRepositoryAsync _productRepository;
            public DeleteProductByIdCommandHandler(
                IProductRedisCacheAsync productRedisCache,
                IProductElasticAsync productElastic,
                IProductRepositoryAsync productRepository
                )
            {
                _productRedisCache = productRedisCache;
                _productElastic = productElastic;
                _productRepository = productRepository;
            }
            public async Task<Response<int>> Handle(DeleteProductByIdCommand command, CancellationToken cancellationToken)
            {
                var product = await _productRepository.GetByIdAsync(command.Id);
                if (product == null) throw new ApiException($"Product Not Found.");
                await _productRepository.DeleteAsync(product);
                await _productRedisCache.RemoveAsync(product.Barcode);
                await _productElastic.RemoveProductAsync(product.Id.ToString(), "product");
                return new Response<int>(product.Id);
            }
        }
    }
}
