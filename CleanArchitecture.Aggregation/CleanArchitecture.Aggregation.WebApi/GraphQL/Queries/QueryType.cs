using CleanArchitecture.Aggregation.WebApi.GraphQL.Queries.Types;
using HotChocolate.Types;

namespace CleanArchitecture.Aggregation.WebApi.GraphQL.Queries
{
    public class QueryType : ObjectType<Query>
    {
        protected override void Configure(IObjectTypeDescriptor<Query> descriptor)
        {
            descriptor.Field(f => f.GetProducts(default!))
                .Name("getProducts")
                .Type<ListType<ProductType>>()
                .Description("Danh sách sản phẩm");

            descriptor.Field(f => f.GetSuperheroes(default!))
                .Name("getSuperhero")
                .Type<ListType<SuperheroType>>()
                .Description("Danh sách Superhero");

            descriptor.Field(f => f.GetSuperheroById(default!,default!))
                .Name("getSuperheroById")
                .Type<SuperheroType>()
                .Description("Chi tiết Superhero");
        }
    }
}
