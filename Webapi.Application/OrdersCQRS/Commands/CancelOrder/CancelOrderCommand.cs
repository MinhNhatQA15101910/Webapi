using Webapi.Application.Common.Interfaces.MediatR;
using Webapi.SharedKernel.DTOs.Orders;

namespace Webapi.Application.OrdersCQRS.Commands.CancelOrder;

public record CancelOrderCommand(Guid OrderId) : ICommand<OrderDto>;
