using HotChocolate.Types;

namespace CleanArchitecture.Aggregation.WebApi.GraphQL.Mutations
{
    public class MutationType : ObjectType<Mutation>
    {
        protected override void Configure(IObjectTypeDescriptor<Mutation> descriptor)
        {

        }
    }
}
