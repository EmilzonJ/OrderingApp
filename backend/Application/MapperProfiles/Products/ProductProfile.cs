using Application.Features.Products.Commands.AddProduct;
using Application.Features.Products.Notifications.AddProduct;
using Application.Features.Products.Queries.GetProductJoinCategories;
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
            CreateMap<Product, AddProductNotification>().ReverseMap();
        }
    }
}