using Application.Commands.Products;
using Application.Features.Products.Queries.GetProductList;
using AutoMapper;
using Domain.Entities;

namespace Application.MapperProfiles.Products
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<Product, AddProductCommand>().ReverseMap();
        }
    }
}