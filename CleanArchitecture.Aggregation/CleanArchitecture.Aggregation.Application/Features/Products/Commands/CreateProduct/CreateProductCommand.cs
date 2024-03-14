using AutoMapper;
using CleanArchitecture.Aggregation.Application.Interfaces.Repositories.Database;
using CleanArchitecture.Aggregation.Application.Interfaces.Repositories.Elastic;
using CleanArchitecture.Aggregation.Application.Interfaces.Repositories.RedisCache;
using CleanArchitecture.Aggregation.Application.Wrappers;
using CleanArchitecture.Aggregation.Domain.Entities;
using MediatR;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArchitecture.Aggregation.Application.Features.Products.Commands.CreateProduct
{
    public partial class CreateProductCommand : IRequest<Response<Product>>
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Barcode { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public decimal Rate { get; set; }
    }
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Response<Product>>
    {
        private readonly IProductRedisCacheAsync _productRedisCache;
        private readonly IProductRepositoryAsync _productRepository;
        private readonly IProductElasticAsync _productElastic;
        private readonly IMapper _mapper;
        public CreateProductCommandHandler(
            IProductRedisCacheAsync productRedisCache,
            IProductRepositoryAsync productRepository,
            IProductElasticAsync productElastic,
            IMapper mapper)
        {
            _productElastic = productElastic;
            _productRedisCache = productRedisCache;
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<Response<Product>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var product = _mapper.Map<Product>(request);
            await _productRepository.AddAsync(product);
            await _productRedisCache.AddAsync(product.Barcode, product, TimeSpan.FromDays(1));
            await _productElastic.AddProductAsync(product, "product");
            return new Response<Product>(product);
        }
    }
}
