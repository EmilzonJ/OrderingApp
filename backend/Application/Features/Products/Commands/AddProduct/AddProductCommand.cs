using MediatR;

namespace Application.Features.Products.Commands.AddProduct
{
    public class AddProductCommand : IRequest
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Size { get; set; }
    }
}