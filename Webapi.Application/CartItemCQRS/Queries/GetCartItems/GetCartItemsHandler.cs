using AutoMapper;
using Microsoft.AspNetCore.Http;
using Webapi.Application.Common.Extensions;
using Webapi.Application.Common.Interfaces.MediatR;
using Webapi.Domain.Interfaces;
using Webapi.SharedKernel.DTOs.CartItem;

namespace Webapi.Application.CartItemCQRS.Queries.GetCartItems;

public class GetCartItemsHandler(
    IHttpContextAccessor httpContextAccessor,
    IUnitOfWork unitOfWork,
    IMapper mapper
) : IQueryHandler<GetCartItemsQuery, IEnumerable<CartItemDto>>
{
    public async Task<IEnumerable<CartItemDto>> Handle(GetCartItemsQuery request, CancellationToken cancellationToken)
    {
        var userId = httpContextAccessor.HttpContext.User.GetUserId();

        // Get cart items with product details
        var cartItems = await unitOfWork.CartItemRepository.GetCartItemsWithDetailsAsync(userId, cancellationToken);

        // Map to DTOs
        return mapper.Map<IEnumerable<CartItemDto>>(cartItems);
    }
}
