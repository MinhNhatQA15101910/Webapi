using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Webapi.Application.OrdersCQRS.Commands.CreateOrder;
using Webapi.Application.OrdersCQRS.Queries.GetOrderById;
using Webapi.Application.OrdersCQRS.Queries.GetOrders;
using Webapi.Presentation.Extensions;
using Webapi.SharedKernel.DTOs.Orders;
using Webapi.SharedKernel.Params;

namespace Webapi.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class OrdersController(IMediator mediator) : ControllerBase
{
    [HttpGet("{orderId}")]
    public async Task<ActionResult<OrderDto>> GetOrderById(Guid orderId)
    {
        return await mediator.Send(new GetOrderByIdQuery(orderId));
    }

    [HttpGet]
    public async Task<IEnumerable<OrderDto>> GetOrders([FromQuery] OrderParams orderParams)
    {
        var orders = await mediator.Send(new GetOrdersQuery(orderParams));

        Response.AddPaginationHeader(orders);

        return orders;
    }

    [HttpPost]
    public async Task<ActionResult<OrderDto>> CreateOrder(CreateOrderDto createOrderDto)
    {
        var order = await mediator.Send(new CreateOrderCommand(createOrderDto));
        return CreatedAtAction(nameof(GetOrderById), new { orderId = order.Id }, order);
    }
}
