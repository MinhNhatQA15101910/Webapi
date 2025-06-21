using Webapi.Application.Common.Interfaces.MediatR;
using Webapi.SharedKernel.DTOs.Orders;
using Webapi.SharedKernel.Helpers;
using Webapi.SharedKernel.Params;

namespace Webapi.Application.OrdersCQRS.Queries.GetOrders;

public record GetOrdersQuery(OrderParams OrderParams) : IQuery<PagedList<OrderDto>>;
