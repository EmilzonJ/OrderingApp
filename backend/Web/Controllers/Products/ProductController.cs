using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Web.Commands.Products;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Web.Commands.Shared;
using Web.DTOs.Products.Requests;
using Web.DTOs.Products.Responses;
using Web.DTOs.Shared;
using Web.Hubs;

namespace Web.Controllers.Products
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class ProductController : Controller
    {
        private readonly IMediator _mediator;

        public ProductController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IEnumerable<GetProductResponse>> GetProducts()
        {
            return await _mediator.Send(new GetProductsCommand());
        }

        [HttpGet("{idProduct:guid}")]
        public async Task<GetProductResponse> GetProductById(Guid idProduct)
        {
            return await _mediator.Send(new GetProductByIdCommand(idProduct));
        }

        [HttpPost]
        public async Task<AddProductReponse> AddProduct([FromBody] AddProductRequest request)
        {
            return await _mediator.Send(new AddProductCommand(request));
        }

        [HttpPut]
        public async Task<GetProductResponse> UpdateProduct([FromBody] UpdateProductRequest request)
        {
            return await _mediator.Send(new UpdateProductCommand(request));
        }

        [HttpDelete("{idProduct:guid}")]
        public async Task<DeleteReponse> DeleteProduct(Guid idProduct)
        {
            await _mediator.Send(new DeleteCommand(idProduct));

            return new DeleteReponse
            {
                Message = "Producto eliminado con exito"
            };
        }
    }
}