using AutoMapper;
using CleanArchitecture.Aggregation.Application.Features.Products.Commands.CreateProduct;
using CleanArchitecture.Aggregation.Application.Features.Products.Queries.GetAllProducts;
using CleanArchitecture.Aggregation.Application.TypeInputs;
using CleanArchitecture.Aggregation.Domain.Entities;

namespace CleanArchitecture.Aggregation.Application.Mappings
{
    public class GeneralProfile : Profile
    {
        public GeneralProfile()
        {
            CreateMap<Product, GetAllProductsViewModel>().ReverseMap();
            CreateMap<CreateProductCommand, Product>();
            CreateMap<ProductTypeInput, Product>().ReverseMap();
            CreateMap<GetAllProductsQuery, GetAllProductsParameter>();

            CreateMap<Superhero, SuperHeroTypeInput>().ReverseMap();
            CreateMap<Movie, MovieTypeInput>().ReverseMap();
            CreateMap<Superpower, SuperPowerTypeInput>().ReverseMap();
        }
    }
}
