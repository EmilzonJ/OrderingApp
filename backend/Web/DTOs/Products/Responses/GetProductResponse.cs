using System;

namespace Web.DTOs.Products.Responses
{
    public class GetProductResponse
    {
        public virtual Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Size { get; set; }
    }
}