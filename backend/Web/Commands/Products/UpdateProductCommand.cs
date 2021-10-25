using Domain.Entities;
using MediatR;

namespace Web.Commands.Products
{
    public class UpdateProductCommand : IRequest<Product>
    {
        public Product Product { get; set; }

        public UpdateProductCommand(Product product)
        {
            Product = product;
        }
    }
}