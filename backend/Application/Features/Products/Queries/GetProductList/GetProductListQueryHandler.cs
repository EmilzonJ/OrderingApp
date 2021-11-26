using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Domain;
using Domain.CustomExceptions;
using Domain.Entities;
using Domain.Repositories;
using MediatR;

namespace Application.Features.Products.Queries.GetProductList
{
    public class GetProductListQueryHandler : IRequestHandler<GetProductListQuery, IEnumerable<ProductDto>>
    {
        private readonly IUnitOfWork _db;
        private readonly IMapper _mapper;

        public GetProductListQueryHandler(IMapper mapper, IUnitOfWork db)
        {
            _mapper = mapper;
            _db = db;
        }

        public async Task<IEnumerable<ProductDto>> Handle(GetProductListQuery request, CancellationToken cancellationToken)
        {
            var products = await _db.Products.GetAllAsync();
            var categories = await _db.Categories.GetAllAsync();

            if (products != null) return _mapper.Map<IEnumerable<ProductDto>>(products);
            
            throw new ApiException(HttpStatusCode.NotFound, new {message = "No existen productos"});
        }
    }
}