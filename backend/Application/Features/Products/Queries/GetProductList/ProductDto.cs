using System;

namespace Application.Features.Products.Queries.GetProductList
{
    public class ProductDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Size { get; set; }
    }
}