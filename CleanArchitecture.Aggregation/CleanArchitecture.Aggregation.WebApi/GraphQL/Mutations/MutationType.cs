using CleanArchitecture.Aggregation.WebApi.GraphQL.Queries.Types;
using HotChocolate.Types;

namespace CleanArchitecture.Aggregation.WebApi.GraphQL.Mutations
{
    public class MutationType : ObjectType<Mutation>
    {
        protected override void Configure(IObjectTypeDescriptor<Mutation> descriptor)
        {
            descriptor.Field(f => f.CreateProductAsync(default!, default!, default))
                .Name("createProduct")
                .Type<ProductType>()
                .Description("Tạo sản phẩm");

            descriptor.Field(f => f.CreateSuperHero(default!, default!,default!, default))
                .Name("createSuperHero")
                //.Type<ProductType>()
                .Description("Tạo superhero");

            descriptor.Field(f => f.DeleteSuperHero(default!,default!))
                .Name("deleteSuperHero")
                .Description("");
        }
    }
}
