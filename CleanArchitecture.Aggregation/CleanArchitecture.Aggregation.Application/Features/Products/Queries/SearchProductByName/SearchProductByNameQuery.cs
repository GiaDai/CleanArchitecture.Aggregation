using CleanArchitecture.Aggregation.Application.Interfaces.Repositories;
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
            private readonly IProductRepositoryAsync _productRepository;
            public SearchProductByNameQueryHandler(IProductRepositoryAsync productRepository)
            {
                _productRepository = productRepository;
            }
            public async Task<Response<List<Product>>> Handle(SearchProductByNameQuery query, CancellationToken cancellationToken)
            {
                var products = await _productRepository.SearchByNameAsync(query.SearchKey);
                return new Response<List<Product>>(products.ToList());
            }
        }
    }
}
