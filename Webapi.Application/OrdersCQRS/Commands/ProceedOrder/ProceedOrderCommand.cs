using Webapi.Application.Common.Interfaces.MediatR;
using Webapi.SharedKernel.DTOs.Orders;

namespace Webapi.Application.OrdersCQRS.Commands.ProceedOrder;

public record ProceedOrderCommand(Guid OrderId) : ICommand<OrderDto>;
