using MediatR;
using Microsoft.AspNetCore.Mvc;
using Webapi.Application.ProductSizeCQRS.Commands.CreateProductSize;
using Webapi.Application.ProductSizeCQRS.Commands.DeleteProductSize;
using Webapi.Application.ProductSizeCQRS.Commands.UpdateProductSize;
using Webapi.Application.ProductSizeCQRS.Queries.GetProductSizeById;
using Webapi.Application.ProductSizeCQRS.Queries.GetProductSizes;
using Webapi.Application.ProductSizeCQRS.Queries.GetProductSizesByProductId;
using Webapi.Presentation.Extensions;
using Webapi.SharedKernel.DTOs;
using Webapi.SharedKernel.Params;

namespace Webapi.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductSizesController(IMediator mediator) : ControllerBase
{
    // GET: api/ProductSizes
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductSizeDto>>> GetProductSizes([FromQuery] ProductSizeParams productSizeParams)
    {
        var productSizes = await mediator.Send(new GetProductSizesQuery(productSizeParams));

        Response.AddPaginationHeader(productSizes);

        return Ok(productSizes);
    }

    // GET: api/ProductSizes/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<ProductSizeDto>> GetProductSize(Guid id)
    {
        var productSize = await mediator.Send(new GetProductSizeByIdQuery(id));
        return Ok(productSize);
    }

    // GET: api/ProductSizes/product/{productId}
    [HttpGet("product/{productId}")]
    public async Task<ActionResult<IEnumerable<ProductSizeDto>>> GetProductSizesByProduct(Guid productId)
    {
        var productSizes = await mediator.Send(new GetProductSizesByProductIdQuery(productId));
        return Ok(productSizes);
    }

    // POST: api/ProductSizes
    [HttpPost]
    public async Task<ActionResult<ProductSizeDto>> CreateProductSize([FromBody] CreateProductSizeDto productSizeDto)
    {
        var productSize = await mediator.Send(new CreateProductSizeCommand(productSizeDto));

        return CreatedAtAction(
            nameof(GetProductSize),
            new { id = productSize.Id },
            productSize
        );
    }

    // PUT: api/ProductSizes/{id}
    [HttpPut("{id}")]
    public async Task<ActionResult<ProductSizeDto>> UpdateProductSize(Guid id, [FromBody] UpdateProductSizeDto productSizeDto)
    {
        var productSize = await mediator.Send(new UpdateProductSizeCommand(id, productSizeDto));
        return Ok(productSize);
    }

    // DELETE: api/ProductSizes/{id}
    [HttpDelete("{id}")]
    public async Task<ActionResult<ProductSizeDto>> DeleteProductSize(Guid id)
    {
        var productSize = await mediator.Send(new DeleteProductSizeCommand(id));
        return Ok(productSize);
    }
}
