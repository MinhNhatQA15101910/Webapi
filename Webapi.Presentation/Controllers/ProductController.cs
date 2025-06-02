using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Webapi.Application.ProductCQRS.Commands.AddProductPhoto;
using Webapi.Application.ProductCQRS.Commands.DeleteProduct;
using Webapi.Application.ProductCQRS.Commands.DeleteProductPhoto;
using Webapi.Application.ProductCQRS.Commands.SetMainProductPhoto;
using Webapi.Application.ProductCQRS.Queries.GetProductById;
using Webapi.Application.ProductCQRS.Queries.GetProductPhotos;
using Webapi.Application.ProductCQRS.Queries.GetProducts;
using Webapi.Application.ProductCQRS.Commands.CreateProduct;
using Webapi.Application.ProductCQRS.Commands.UpdateProduct;
using Webapi.Presentation.Extensions;
using Webapi.SharedKernel.DTOs;
using Webapi.SharedKernel.Params;

namespace Webapi.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductController(IMediator mediator) : ControllerBase
{
    // GET: api/Product
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts([FromQuery] ProductParams productParams)
    {
        var products = await mediator.Send(new GetProductsQuery(productParams));
        
        Response.AddPaginationHeader(products);
        
        return Ok(products);
    }
    
    // GET: api/Product/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<ProductDto>> GetProduct(Guid id)
    {
        var product = await mediator.Send(new GetProductByIdQuery(id));
        return Ok(product);
    }
    
    // POST: api/Product
    [HttpPost]
    // [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ProductDto>> CreateProduct([FromForm] CreateProductDto createProductDto)
    {
        var product = await mediator.Send(new CreateProductCommand(createProductDto));
        
        return CreatedAtAction(
            nameof(GetProduct),
            new { id = product.Id },
            product
        );
    }
    
    // PUT: api/Product/{id}
    [HttpPut("{id}")]
    // [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ProductDto>> UpdateProduct(Guid id, [FromBody] UpdateProductDto updateProductDto)
    {
        var product = await mediator.Send(new UpdateProductCommand(id, updateProductDto));
        return Ok(product);
    }
    
    // DELETE: api/Product/{id}
    [HttpDelete("{id}")]
    // [Authorize(Roles = "Admin")]
    public async Task<ActionResult> DeleteProduct(Guid id)
    {
        await mediator.Send(new DeleteProductCommand(id));
        return NoContent();
    }
    
    // Photo endpoints with different route patterns
    
    // GET: api/Product/{id}/photos
    [HttpGet("{id}/photos")]
    public async Task<ActionResult<IEnumerable<ProductPhotoDto>>> GetProductPhotos(Guid id)
    {
        var photos = await mediator.Send(new GetProductPhotosQuery(id));
        return Ok(photos);
    }
    
    // POST: api/Product/{id}/photos
    [HttpPost("{id}/photos")]
    // [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ProductPhotoDto>> AddProductPhoto(Guid id, IFormFile file)
    {
        var photo = await mediator.Send(new AddProductPhotoCommand(id, file));
        
        return CreatedAtAction(
            nameof(GetProduct),
            new { id },
            photo
        );
    }
    
    // PUT: api/Product/{id}/photos/{photoId}/set-main
    [HttpPut("{id}/photos/{photoId}/set-main")]
    // [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ProductPhotoDto>> SetMainProductPhoto(Guid id, Guid photoId)
    {
        var photo = await mediator.Send(new SetMainProductPhotoCommand(id, photoId));
        return Ok(photo);
    }
    
    // DELETE: api/Product/{id}/photos/{photoId}
    [HttpDelete("{id}/photos/{photoId}")]
    // [Authorize(Roles = "Admin")]
    public async Task<ActionResult> DeleteProductPhoto(Guid id, Guid photoId)
    {
        await mediator.Send(new DeleteProductPhotoCommand(id, photoId));
        return NoContent();
    }
}