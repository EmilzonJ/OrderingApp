using Domain.Entities;
using MediatR;
using Web.DTOs.Products.Requests;
using Web.DTOs.Products.Responses;

namespace Web.Commands.Products
{
    public class UpdateProductCommand : IRequest<GetProductResponse>
    {
        public UpdateProductRequest Product { get; set; }

        public UpdateProductCommand(UpdateProductRequest product)
        {
            Product = product;
        }
    }
}