using System;
using MediatR;
using Web.DTOs.Products.Responses;

namespace Web.Commands.Products
{
    public class GetProductByIdCommand : IRequest<GetProductResponse>
    {
        public Guid Id { get; set; }

        public GetProductByIdCommand(Guid id)
        {
            Id = id;
        }
    }
}