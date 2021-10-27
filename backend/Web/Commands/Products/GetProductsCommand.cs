using System.Collections.Generic;
using Domain.Entities;
using MediatR;
using Web.DTOs.Products.Responses;

namespace Web.Commands.Products
{
    public class GetProductsCommand : IRequest<IEnumerable<GetProductResponse>>
    {
    }
}