using AutoMapper;
using CleanArchitecture.Aggregation.Application.Features.Products.Commands.CreateProduct;
using CleanArchitecture.Aggregation.Application.Interfaces.Repositories.Database;
using CleanArchitecture.Aggregation.Application.Interfaces.Repositories.Elastic;
using CleanArchitecture.Aggregation.Application.Interfaces.Repositories.RedisCache;
using CleanArchitecture.Aggregation.Application.Wrappers;
using CleanArchitecture.Aggregation.Domain.Entities;
using MediatR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArchitecture.Aggregation.Application.Features.Products.Commands.CreateProductRange
{
    public class CreateProductRangeCommand : IRequest<Response<int>>
    {
        public List<CreateProductCommand> Products { get; set; }
    }

    public class CreateProductRangeCommandHandler : IRequestHandler<CreateProductRangeCommand, Response<int>>
    {
        private readonly IProductRedisCacheAsync _productRedisCache;
        private readonly IProductRepositoryAsync _productRepository;
        private readonly IProductElasticAsync _productElastic;
        private readonly IMapper _mapper;
        public CreateProductRangeCommandHandler(
            IProductElasticAsync productElastic,
            IProductRedisCacheAsync productRedisCache,
            IProductRepositoryAsync productRepository,
            IMapper mapper
            )
        {
            _productElastic = productElastic;
            _productRedisCache = productRedisCache;
            _productRepository = productRepository;
            _mapper = mapper;
        }
        public async Task<Response<int>> Handle(CreateProductRangeCommand request, CancellationToken cancellationToken)
        {
            var products = _mapper.Map<List<Product>>(request.Products);
            var exitingProduct = await _productRepository.AddRangeAsync(products);
            var newProducts = products.Where(p => !exitingProduct.Any(ep => ep.Barcode == p.Barcode)).ToList();
            if (newProducts.Count > 0)
            {
                Console.WriteLine(JsonConvert.SerializeObject(newProducts));
                await _productRedisCache.AddRangeAsync(newProducts, TimeSpan.FromDays(1));
                await _productElastic.AddRangeAsync(newProducts, "product");
            }
            return new Response<int>(newProducts.Count);
        }
    }
}
