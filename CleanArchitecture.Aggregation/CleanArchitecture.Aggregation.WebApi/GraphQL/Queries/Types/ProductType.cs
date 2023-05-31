using CleanArchitecture.Aggregation.Domain.Entities;
using HotChocolate.Types;

namespace CleanArchitecture.Aggregation.WebApi.GraphQL.Queries.Types
{
    public class ProductType : ObjectType<Product>
    {
        protected override void Configure(IObjectTypeDescriptor<Product> descriptor)
        {
        }
    }
}
