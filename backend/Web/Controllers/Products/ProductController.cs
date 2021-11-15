using System.Threading.Tasks;
using Application.Commands.Products;
using Application.Features.Products.Queries.GetProductList;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> GetProducts()
        {
            var products = await _mediator.Send(new GetProductListQuery());
            
            return Ok(products);
        }
        //
        // [HttpGet("{idProduct:guid}")]
        // public async Task<GetProductResponse> GetProductById(Guid idProduct)
        // {
        //     return await _mediator.Send(new GetProductByIdCommand(idProduct));
        // }

        [HttpPost]
        public async Task<ActionResult> AddProduct([FromBody] AddProductCommand command)
        {
            await _mediator.Send(command);
            return NoContent();
        }

        // [HttpPut]
        // public async Task<GetProductResponse> UpdateProduct([FromBody] UpdateProductRequest request)
        // {
        //     return await _mediator.Send(new UpdateProductCommand(request));
        // }
        //
        // [HttpDelete("{idProduct:guid}")]
        // public async Task<DeleteReponse> DeleteProduct(Guid idProduct)
        // {
        //     await _mediator.Send(new DeleteCommand(idProduct));
        //
        //     return new DeleteReponse
        //     {
        //         Message = "Producto eliminado con exito"
        //     };
        // }
    }
}