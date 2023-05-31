using AutoMapper;
using CleanArchitecture.Aggregation.Application.Features.Products.Commands.CreateProduct;
using CleanArchitecture.Aggregation.Application.Features.Products.Queries.GetAllProducts;
using CleanArchitecture.Aggregation.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanArchitecture.Aggregation.Application.Mappings
{
    public class GeneralProfile : Profile
    {
        public GeneralProfile()
        {
            CreateMap<Product, GetAllProductsViewModel>().ReverseMap();
            CreateMap<CreateProductCommand, Product>();
            CreateMap<GetAllProductsQuery, GetAllProductsParameter>();
        }
    }
}
