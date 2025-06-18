using Webapi.Application.Common.Interfaces.MediatR;
using Webapi.SharedKernel.DTOs.Orders;

namespace Webapi.Application.OrdersCQRS.Commands.CreateOrder;

public record CreateOrderCommand(CreateOrderDto CreateOrderDto) : ICommand<OrderDto>;
