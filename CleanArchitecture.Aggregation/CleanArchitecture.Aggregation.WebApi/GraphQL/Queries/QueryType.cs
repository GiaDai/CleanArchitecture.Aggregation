using CleanArchitecture.Aggregation.WebApi.GraphQL.Queries.Types;
using HotChocolate.Types;

namespace CleanArchitecture.Aggregation.WebApi.GraphQL.Queries
{
    public class QueryType : ObjectType<Query>
    {
        protected override void Configure(IObjectTypeDescriptor<Query> descriptor)
        {
            descriptor.Field(f => f.GetProducts(default!))
                .Name("GetProducts")
                .Type<ListType<ProductType>>()
                .Description("Danh sách sản phẩm");
        }
    }
}
