using System;
using Domain.Entities;
using MediatR;

namespace Web.Commands.Products
{
    public class GetProductByIdCommand : IRequest<Product>
    {
        public Guid Id { get; set; }

        public GetProductByIdCommand(Guid id)
        {
            Id = id;
        }
    }
}