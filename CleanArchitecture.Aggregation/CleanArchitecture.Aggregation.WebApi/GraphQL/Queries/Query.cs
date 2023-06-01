using CleanArchitecture.Aggregation.Domain.Entities;
using CleanArchitecture.Aggregation.Infrastructure.Persistence.Contexts;
using HotChocolate;
using HotChocolate.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace CleanArchitecture.Aggregation.WebApi.GraphQL.Queries
{
    public class Query
    {
        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public IQueryable<Product> GetProducts([Service] ApplicationDbContext context)
        {
            return context.Products;
        }

        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public IQueryable<Superhero> GetSuperheroes([Service] ApplicationDbContext context) =>
            context.Superheros
            .Include(x => x.Superpowers)
            .Include(x => x.Movies)
            .AsSplitQuery();

        public Superhero GetSuperheroById([Service] ApplicationDbContext context, int id) =>
            context.Superheros
            .Include(x => x.Superpowers)
            .Include(x => x.Movies)
            .Where(x => x.Id.Equals(id))
            .FirstOrDefault(x => x.Id.Equals(id));
    }
}
