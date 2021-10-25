using System.Collections.Generic;
using AutoMapper;
using Domain.Entities;
using Web.DTOs.Products.Requests;
using Web.DTOs.Products.Responses;

namespace Web.MapperProfiles.Products
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, GetProductResponse>();
            CreateMap<AddProductRequest, Product>();
            CreateMap<Product, AddProductReponse>();
            CreateMap<UpdateProductRequest, Product>();
        }
    }
}