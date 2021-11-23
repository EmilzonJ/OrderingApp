using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Features.Products.Queries.GetProductList;
using AutoMapper;
using Domain;
using MediatR;

namespace Application.Features.Products.Queries.GetProductJoinCategories
{
    public class GetProductsCategoriesHandler : IRequestHandler<GetProductsCategories, ProductsCategoriesDto>
    {
        private readonly IUnitOfWork _db;
        private readonly IMapper _mapper;

        public GetProductsCategoriesHandler(IUnitOfWork db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<ProductsCategoriesDto> Handle(GetProductsCategories request, CancellationToken cancellationToken)
        {
            var categories = await _db.Categories.GetAllAsync();
            var products = await _db.Products.GetAllAsync();

            var categoriesDto = _mapper.Map<IEnumerable<CategoryDto>>(categories);
            var productsDto = _mapper.Map<IEnumerable<ProductDto>>(products);
            
            return new ProductsCategoriesDto { Categories = categoriesDto, Products = productsDto };
        }
    }
}