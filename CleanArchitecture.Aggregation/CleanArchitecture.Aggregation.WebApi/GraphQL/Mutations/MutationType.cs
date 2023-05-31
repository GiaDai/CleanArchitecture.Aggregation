using CleanArchitecture.Aggregation.WebApi.GraphQL.Queries.Types;
using HotChocolate.Types;

namespace CleanArchitecture.Aggregation.WebApi.GraphQL.Mutations
{
    public class MutationType : ObjectType<Mutation>
    {
        protected override void Configure(IObjectTypeDescriptor<Mutation> descriptor)
        {
            descriptor.Field(f => f.CreateProductAsync(default!, default!, default!))
                .Name("createProduct")
                .Type<ProductType>()
                .Description("Tạo sản phẩm");
        }
    }
}
