using Application.Features.Products.Queries.GetProductJoinCategories;
using AutoMapper;

namespace Application.MapperProfiles.Category
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<Domain.Entities.Category, CategoryDto>().ReverseMap();
        }
    }
}