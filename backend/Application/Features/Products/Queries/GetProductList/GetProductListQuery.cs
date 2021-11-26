using System.Collections.Generic;
using MediatR;

namespace Application.Features.Products.Queries.GetProductList
{
    public class GetProductListQuery : IRequest<IEnumerable<ProductDto>>
    {
        
    }
}