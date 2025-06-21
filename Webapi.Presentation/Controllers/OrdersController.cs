using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Webapi.Application.OrdersCQRS.Commands.CancelOrder;
using Webapi.Application.OrdersCQRS.Commands.CreateOrder;
using Webapi.Application.OrdersCQRS.Commands.ProceedOrder;
using Webapi.Application.OrdersCQRS.Queries.GetOrderById;
using Webapi.Application.OrdersCQRS.Queries.GetOrders;
using Webapi.Presentation.Extensions;
using Webapi.SharedKernel.DTOs.Orders;
using Webapi.SharedKernel.Params;

namespace Webapi.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrdersController(IMediator mediator) : ControllerBase
{
    [Authorize]
    [HttpGet("{orderId}")]
    public async Task<ActionResult<OrderDto>> GetOrderById(Guid orderId)
    {
        return await mediator.Send(new GetOrderByIdQuery(orderId));
    }

    [Authorize]
    [HttpGet]
    public async Task<IEnumerable<OrderDto>> GetOrders([FromQuery] OrderParams orderParams)
    {
        var orders = await mediator.Send(new GetOrdersQuery(orderParams));

        Response.AddPaginationHeader(orders);

        return orders;
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<OrderDto>> CreateOrder(CreateOrderDto createOrderDto)
    {
        var order = await mediator.Send(new CreateOrderCommand(createOrderDto));
        return CreatedAtAction(nameof(GetOrderById), new { orderId = order.Id }, order);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("proceed/{orderId}")]
    public async Task<ActionResult<OrderDto>> ProceedOrder(Guid orderId)
    {
        return await mediator.Send(new ProceedOrderCommand(orderId));
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("cancel/{orderId}")]
    public async Task<ActionResult<OrderDto>> CancelOrder(Guid orderId)
    {
        return await mediator.Send(new CancelOrderCommand(orderId));
    }
}
