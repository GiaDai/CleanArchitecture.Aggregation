using CleanArchitecture.Aggregation.Domain.Entities;
using HotChocolate.Types;

namespace CleanArchitecture.Aggregation.WebApi.GraphQL.Queries.Types
{
    public class SuperheroType : ObjectType<Superhero>
    {
        protected override void Configure(IObjectTypeDescriptor<Superhero> descriptor)
        {
        }
    }
}
