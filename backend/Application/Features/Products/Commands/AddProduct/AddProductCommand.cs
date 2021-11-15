using MediatR;

namespace Application.Commands.Products
{
    public class AddProductCommand : IRequest
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Size { get; set; }
    }
}