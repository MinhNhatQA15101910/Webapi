using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Webapi.Application.OrdersCQRS.Commands.CreateOrder;
using Webapi.SharedKernel.DTOs.Orders;

namespace Webapi.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class OrdersController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<OrderDto>> CreateOrder(CreateOrderDto createOrderDto)
    {
        var order = await mediator.Send(new CreateOrderCommand(createOrderDto));
        return Ok(order);
    }
}
