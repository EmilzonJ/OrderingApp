using MediatR;
using Web.DTOs.Products.Requests;
using Web.DTOs.Products.Responses;

namespace Web.Commands.Products
{
    public class AddProductCommand : IRequest<AddProductReponse>
    {
        public AddProductRequest Product { get; set; }
        
        public AddProductCommand(AddProductRequest product)
        {
            Product = product;
        }
    }
}