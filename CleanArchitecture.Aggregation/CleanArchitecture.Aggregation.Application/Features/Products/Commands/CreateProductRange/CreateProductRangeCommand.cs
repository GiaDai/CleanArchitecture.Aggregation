using AutoMapper;
using CleanArchitecture.Aggregation.Application.Features.Products.Commands.CreateProduct;
using CleanArchitecture.Aggregation.Application.Interfaces.Repositories;
using CleanArchitecture.Aggregation.Application.Wrappers;
using CleanArchitecture.Aggregation.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
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
        private readonly IProductRepositoryAsync _productRepository;
        private readonly IMapper _mapper;
        public CreateProductRangeCommandHandler(
            IProductRepositoryAsync productRepository,
            IMapper mapper
            )
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }
        public async Task<Response<int>> Handle(CreateProductRangeCommand request, CancellationToken cancellationToken)
        {
            var products = _mapper.Map<List<Product>>(request.Products);
            var total = await _productRepository.AddRangeAsync(products);
            return new Response<int>(total);
        }
    }
}
