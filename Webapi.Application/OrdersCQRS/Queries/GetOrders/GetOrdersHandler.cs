using Microsoft.AspNetCore.Http;
using Webapi.Application.Common.Extensions;
using Webapi.Application.Common.Interfaces.MediatR;
using Webapi.Domain.Interfaces;
using Webapi.SharedKernel.DTOs.Orders;
using Webapi.SharedKernel.Helpers;

namespace Webapi.Application.OrdersCQRS.Queries.GetOrders;

public class GetOrdersHandler(
    IHttpContextAccessor httpContextAccessor,
    IUnitOfWork unitOfWork
) : IQueryHandler<GetOrdersQuery, PagedList<OrderDto>>
{
    public async Task<PagedList<OrderDto>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
    {
        var roles = httpContextAccessor.HttpContext!.User.GetRoles();
        if (roles.Contains("Admin"))
        {
            return await unitOfWork.OrderRepository.GetOrdersAsync(null, request.OrderParams, cancellationToken);
        }

        var userId = httpContextAccessor.HttpContext.User.GetUserId();
        return await unitOfWork.OrderRepository.GetOrdersAsync(
            userId,
            request.OrderParams,
            cancellationToken
        );
    }
}
