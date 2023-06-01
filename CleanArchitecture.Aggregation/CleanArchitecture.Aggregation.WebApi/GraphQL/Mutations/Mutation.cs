using AppAny.HotChocolate.FluentValidation;
using AutoMapper;
using CleanArchitecture.Aggregation.Application.Interfaces.Repositories;
using CleanArchitecture.Aggregation.Application.TypeInputs;
using CleanArchitecture.Aggregation.Domain.Entities;
using CleanArchitecture.Aggregation.Infrastructure.Persistence.Contexts;
using HotChocolate;
using HotChocolate.Data;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace CleanArchitecture.Aggregation.WebApi.GraphQL.Mutations
{
    public class Mutation
    {
        [UseProjection]
        public async Task<Product> CreateProductAsync(
            [Service] IProductRepositoryAsync _productRepository,
            [Service] IMapper _mapper,
            [UseFluentValidation] ProductTypeInput request
            )
        {
            var _product = _mapper.Map<Product>(request);
            return await _productRepository.AddAsync(_product);
        }

        public async Task<Superhero> CreateSuperHero(
            [Service] ApplicationDbContext context, 
            [Service] IMapper _mapper,
            [Service] ILogger<Mutation> _logger,
            SuperHeroTypeInput request)
        {
            _logger.LogInformation(JsonConvert.SerializeObject(request));
            var superHero = _mapper.Map<Superhero>(request);
            context.Superheros.Add(superHero);
            await context.SaveChangesAsync();
            return superHero;
        }

        public async Task<int> DeleteSuperHero([Service] ApplicationDbContext context,int id)
        {
            var _superHero = await context.Superheros.FindAsync(id);
            if (_superHero == null) throw new GraphQLException("Not found");
            context.Superheros.Remove(_superHero);
            await context.SaveChangesAsync();
            return _superHero.Id;
        }
    }
}
