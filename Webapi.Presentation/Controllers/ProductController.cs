using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Webapi.Application.ProductsCQRS.Commands.CreateProduct;

using Webapi.Presentation.Extensions;
using Webapi.SharedKernel.DTOs;
using Webapi.SharedKernel.Params;

namespace Webapi.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController(IMediator mediator) : ControllerBase
{
    // [HttpGet]
    // public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts([FromQuery] ProductParams productParams)
    // {
    //     var products = await mediator.Send(new GetProductsQuery(productParams));
    //     
    //     Response.AddPaginationHeader(products);
    //     
    //     return Ok(products);
    // }
    //
    // [HttpGet("{id}")]
    // public async Task<ActionResult<ProductDto>> GetProduct(Guid id)
    // {
    //     var product = await mediator.Send(new GetProductByIdQuery(id));
    //     return Ok(product);
    // }
    //
    // [HttpPost]
    // [Authorize(Roles = "Admin")]
    // public async Task<ActionResult<ProductDto>> CreateProduct(CreateProductDto createProductDto)
    // {
    //     var product = await mediator.Send(new CreateProductCommand(createProductDto));
    //     
    //     return CreatedAtAction(
    //         nameof(GetProduct),
    //         new { id = product.Id },
    //         product
    //     );
    // }
    //
    // [HttpPut("{id}")]
    // [Authorize(Roles = "Admin")]
    // public async Task<ActionResult> UpdateProduct(Guid id, UpdateProductDto updateProductDto)
    // {
    //     await mediator.Send(new UpdateProductCommand(id, updateProductDto));
    //     return NoContent();
    // }
    //
    // [HttpDelete("{id}")]
    // [Authorize(Roles = "Admin")]
    // public async Task<ActionResult> DeleteProduct(Guid id)
    // {
    //     await mediator.Send(new DeleteProductCommand(id));
    //     return NoContent();
    // }
    //
    // [HttpPost("{id}/photos")]
    // [Authorize(Roles = "Admin")]
    // public async Task<ActionResult<ProductPhotoDto>> AddProductPhoto(Guid id, IFormFile file)
    // {
    //     var photo = await mediator.Send(new AddProductPhotoCommand(id, file));
    //     
    //     return CreatedAtAction(
    //         nameof(GetProduct),
    //         new { id },
    //         photo
    //     );
    // }
    //
    // [HttpPut("{id}/photos/{photoId}/main")]
    // [Authorize(Roles = "Admin")]
    // public async Task<ActionResult> SetMainProductPhoto(Guid id, Guid photoId)
    // {
    //     await mediator.Send(new SetMainProductPhotoCommand(id, photoId));
    //     return NoContent();
    // }
    //
    // [HttpDelete("{id}/photos/{photoId}")]
    // [Authorize(Roles = "Admin")]
    // public async Task<ActionResult> DeleteProductPhoto(Guid id, Guid photoId)
    // {
    //     await mediator.Send(new DeleteProductPhotoCommand(id, photoId));
    //     return NoContent();
    // }
}