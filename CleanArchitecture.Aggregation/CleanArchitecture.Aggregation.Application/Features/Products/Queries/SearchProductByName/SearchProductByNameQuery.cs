using CleanArchitecture.Aggregation.Application.Interfaces.Repositories.Database;
using CleanArchitecture.Aggregation.Application.Interfaces.Repositories.Elastic;
using CleanArchitecture.Aggregation.Application.Wrappers;
using CleanArchitecture.Aggregation.Domain.Entities;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArchitecture.Aggregation.Application.Features.Products.Queries.SearchProductByName
{
    public class SearchProductByNameQuery : IRequest<Response<List<Product>>>
    {
        public string SearchKey { get; set; }
        public class SearchProductByNameQueryHandler : IRequestHandler<SearchProductByNameQuery, Response<List<Product>>>
        {
            private readonly IProductElasticAsync _productElastic;
            private readonly IProductRepositoryAsync _productRepository;
            public SearchProductByNameQueryHandler(
                IProductRepositoryAsync productRepository,
                IProductElasticAsync productElastic
                )
            {
                _productElastic = productElastic;
                _productRepository = productRepository;
            }
            public async Task<Response<List<Product>>> Handle(SearchProductByNameQuery query, CancellationToken cancellationToken)
            {
                //var products = await _productRepository.SearchByNameAsync(query.SearchKey);
                var productsFromElastic = await _productElastic.SearchByName(query.SearchKey, "product");
                return new Response<List<Product>>(productsFromElastic.ToList());
            }
        }
    }
}
