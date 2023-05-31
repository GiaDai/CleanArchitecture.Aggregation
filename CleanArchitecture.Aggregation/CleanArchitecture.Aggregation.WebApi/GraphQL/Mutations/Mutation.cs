using AppAny.HotChocolate.FluentValidation;
using AutoMapper;
using CleanArchitecture.Aggregation.Application.Interfaces.Repositories;
using CleanArchitecture.Aggregation.Application.TypeInputs;
using CleanArchitecture.Aggregation.Domain.Entities;
using HotChocolate;
using System.Threading.Tasks;

namespace CleanArchitecture.Aggregation.WebApi.GraphQL.Mutations
{
    public class Mutation
    {
        public async Task<Product> CreateProductAsync(
            [Service] IProductRepositoryAsync _productRepository,
            [Service] IMapper _mapper,
            [UseFluentValidation] ProductTypeInput request
            )
        {
            var _product = new Product() { 
                Barcode = request.Barcode,
                Name = request.Name,
                Description = request.Description,
                Rate = request.Rate
            };
            return await _productRepository.AddAsync(_product);
        }
    }
}
