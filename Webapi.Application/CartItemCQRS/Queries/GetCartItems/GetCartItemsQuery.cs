using Webapi.Application.Common.Interfaces.MediatR;
using Webapi.SharedKernel.DTOs.CartItem;

namespace Webapi.Application.CartItemCQRS.Queries.GetCartItems;

public record GetCartItemsQuery() : IQuery<IEnumerable<CartItemDto>>;