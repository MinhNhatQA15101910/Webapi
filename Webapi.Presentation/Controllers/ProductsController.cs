using MediatR;
using Microsoft.AspNetCore.Mvc;
using Webapi.Application.ProductCQRS.Commands.AddProductPhoto;
using Webapi.Application.ProductCQRS.Commands.DeleteProduct;
using Webapi.Application.ProductCQRS.Commands.SetMainProductPhoto;
using Webapi.Application.ProductCQRS.Queries.GetProductById;
using Webapi.Application.ProductCQRS.Queries.GetProductPhotos;
using Webapi.Application.ProductCQRS.Queries.GetProducts;
using Webapi.Application.ProductCQRS.Commands.CreateProduct;
using Webapi.Application.ProductCQRS.Commands.UpdateProduct;
using Webapi.Application.ProductCQRS.Queries.GetAllProductRatings;
using Webapi.Application.ProductCQRS.Queries.GetProductRating;
using Webapi.Presentation.Extensions;
using Webapi.SharedKernel.DTOs.Product;
using Webapi.SharedKernel.DTOs.ProductPhoto;
using Webapi.SharedKernel.Helpers;
using Webapi.SharedKernel.Params;

namespace Webapi.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController(IMediator mediator) : ControllerBase
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
        
        return Ok(product);
    }

    // PUT: api/Product/{id}
    [HttpPut("{id}")]
    // [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ProductDto>> UpdateProduct(Guid id, [FromForm] UpdateProductDto updateProductDto)
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

    // GET: api/Product/{id}/rating
    [HttpGet("{id}/rating")]
    public async Task<ActionResult<ProductRatingDto>> GetProductRating(Guid id)
    {
        return await mediator.Send(new GetProductRatingQuery(id));
    }

    // GET: api/Product/ratings
    [HttpGet("ratings")]
    public async Task<ActionResult<PagedList<ProductRatingDto>>> GetAllProductRatings([FromQuery] ProductParams productParams)
    {
        var result = await mediator.Send(new GetAllProductRatingsQuery(productParams));

        return Ok(result);
    }
}