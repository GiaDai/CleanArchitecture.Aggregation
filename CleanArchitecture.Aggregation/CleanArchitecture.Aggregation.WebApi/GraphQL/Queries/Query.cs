using CleanArchitecture.Aggregation.Domain.Entities;
using CleanArchitecture.Aggregation.Infrastructure.Persistence.Contexts;
using HotChocolate;
using HotChocolate.Data;
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
    }
}
