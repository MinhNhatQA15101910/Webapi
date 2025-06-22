using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Webapi.Application.CartItemCQRS.Commands.AddCartItem;
using Webapi.Application.CartItemCQRS.Commands.ClearCart;
using Webapi.Application.CartItemCQRS.Commands.RemoveCartItem;
using Webapi.Application.CartItemCQRS.Commands.UpdateCartItem;
using Webapi.Application.CartItemCQRS.Queries.GetCartItems;
using Webapi.SharedKernel.DTOs.CartItem;

namespace Webapi.Presentation.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class CartItemsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CartItemDto>>> GetCartItems()
    {
        var query = new GetCartItemsQuery();
        var result = await mediator.Send(query);
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<CartItemDto>> AddCartItem(AddCartItemDto addCartItemDto)
    {
        var command = new AddCartItemCommand(addCartItemDto);
        var result = await mediator.Send(command);
        return CreatedAtAction(nameof(GetCartItems), result);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<CartItemDto>> UpdateCartItem(Guid id, UpdateCartItemDto updateCartItemDto)
    {
        var command = new UpdateCartItemCommand(id, updateCartItemDto);
        var result = await mediator.Send(command);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<CartItemDto>> RemoveCartItem(Guid id)
    {
        var command = new RemoveCartItemCommand(id);
        var result = await mediator.Send(command);
        return Ok(result);
    }

    [HttpDelete]
    public async Task<ActionResult<bool>> ClearCart()
    {
        var command = new ClearCartCommand();
        var result = await mediator.Send(command);
        return Ok(result);
    }
}