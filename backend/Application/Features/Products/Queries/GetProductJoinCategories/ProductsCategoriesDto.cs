using System.Collections.Generic;
using Application.Features.Products.Queries.GetProductList;
using Domain.Entities;

namespace Application.Features.Products.Queries.GetProductJoinCategories
{
    public class ProductsCategoriesDto
    {
        public IEnumerable<ProductDto> Products { get; set; }
        public IEnumerable<CategoryDto> Categories { get; set; }
    }
}