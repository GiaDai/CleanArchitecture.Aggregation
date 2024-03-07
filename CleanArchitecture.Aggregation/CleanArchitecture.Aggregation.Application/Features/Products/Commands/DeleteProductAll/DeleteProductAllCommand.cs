using CleanArchitecture.Aggregation.Application.Interfaces.Repositories.Database;
using CleanArchitecture.Aggregation.Application.Interfaces.Repositories.Elastic;
using CleanArchitecture.Aggregation.Application.Interfaces.Repositories.RedisCache;
using CleanArchitecture.Aggregation.Application.Wrappers;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArchitecture.Aggregation.Application.Features.Products.Commands.DeleteProductAll
{
    public class DeleteProductAllCommand : IRequest<Response<int>>
    {

    }

    public class DeleteProductAllCommandHandler : IRequestHandler<DeleteProductAllCommand, Response<int>>
    {
        private readonly IProductElasticAsync _productElastic;
        private readonly IProductRedisCacheAsync _productRedisCache;
        private readonly IProductRepositoryAsync _productRepository;
        public DeleteProductAllCommandHandler(
            IProductRepositoryAsync productRepository,
            IProductRedisCacheAsync productRedisCache,
            IProductElasticAsync productElastic
            )
        {
            _productRedisCache = productRedisCache;
            _productRepository = productRepository;
            _productElastic = productElastic;
        }
        public async Task<Response<int>> Handle(DeleteProductAllCommand request, CancellationToken cancellationToken)
        {
            await _productRepository.DeleteAllAsync();
            await _productElastic.RemoveAllProductsAsync("product");
            await _productRedisCache.RemoveAllAsync();
            return new Response<int>(1);
        }
    }
}
