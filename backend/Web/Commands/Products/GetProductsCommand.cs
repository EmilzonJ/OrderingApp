using System.Collections.Generic;
using Domain.Entities;
using MediatR;

namespace Web.Commands.Products
{
    public class GetProductsCommand : IRequest<IEnumerable<Product>>
    {
    }
}