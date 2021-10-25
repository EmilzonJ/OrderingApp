using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Web.Commands.Products;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Web.Commands.Shared;
using Web.DTOs.Products.Requests;
using Web.DTOs.Products.Responses;
using Web.DTOs.Shared;

namespace Web.Controllers.Products
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProductController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public ProductController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<GetProductResponse>> GetProducts()
        {
            var products = await _mediator.Send(new GetProductsCommand());

            return _mapper.Map<IEnumerable<GetProductResponse>>(products);
        }

        [HttpGet("{idProduct:guid}")]
        public async Task<GetProductResponse> GetProductById(Guid idProduct)
        {
            var product = await _mediator.Send(new GetProductByIdCommand(idProduct));

            return _mapper.Map<GetProductResponse>(product);
        }

        [HttpPost]
        public async Task<AddProductReponse> AddProduct([FromBody] AddProductRequest request)
        {
            var product = _mapper.Map<Product>(request);

            var response = await _mediator.Send(new AddProductCommand(product));

            return _mapper.Map<AddProductReponse>(response);
        }

        [HttpPut("{idProduct:guid}")]
        public async Task<GetProductResponse> UpdateProduct(Guid idProduct, [FromBody] UpdateProductRequest request)
        {
            var product = _mapper.Map<Product>(request);
            product.Id = idProduct;

            var response = await _mediator.Send(new UpdateProductCommand(product));

            return _mapper.Map<GetProductResponse>(response);
        }

        [HttpDelete("{idProduct:guid}")]
        public async Task<DeleteReponse> DeleteProduct(Guid idProduct)
        {
            await _mediator.Send(new DeleteCommand(idProduct));

            return new DeleteReponse
            {
                IsDeleted = true,
                Message = "Producto eliminado con exito"
            };
        }
    }
}