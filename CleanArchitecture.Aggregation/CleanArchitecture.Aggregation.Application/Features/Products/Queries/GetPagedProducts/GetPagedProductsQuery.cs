using AutoMapper;
using CleanArchitecture.Aggregation.Application.Filters;
using CleanArchitecture.Aggregation.Application.Interfaces.Repositories.Database;
using CleanArchitecture.Aggregation.Application.Wrappers;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CleanArchitecture.Aggregation.Application.Features.Products.Queries.GetPagedProducts
{
    public class GetPagedProductsQuery : IRequest<Response<object>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string Search { get; set; } = "";

        public class GetPagedProductsQueryHandler : IRequestHandler<GetPagedProductsQuery, Response<object>>
        {
            private readonly IMapper _mapper;
            private readonly IProductRepositoryAsync _productRepository;
            public GetPagedProductsQueryHandler(
                IMapper mapper,
                IProductRepositoryAsync productRepository
                )
            {
                _mapper = mapper;
                _productRepository = productRepository; 
            }

            public async Task<Response<object>> Handle(GetPagedProductsQuery request, CancellationToken cancellationToken)
            {
                var _paramProduct = _mapper.Map<RequestParameter>(request);
                var _products = await _productRepository.GetPagedListAsync(_paramProduct);
                if(_products == null) throw new ApplicationException("No products found");
                return new Response<object>(new
                {
                    _products.CurrentPage,
                    _products.TotalPages,
                    _products.PageSize,
                    _products.TotalCount,
                    _products.HasPrevious,
                    _products.HasNext,
                    data = _products
                }, "Products found");
            }
        }
    }
}
