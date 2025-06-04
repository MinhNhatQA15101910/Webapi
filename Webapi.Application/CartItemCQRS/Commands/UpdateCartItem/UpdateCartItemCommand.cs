using Webapi.Application.Common.Interfaces.MediatR;
using Webapi.SharedKernel.DTOs.CartItem;

namespace Webapi.Application.CartItemCQRS.Commands.UpdateCartItem;

public record UpdateCartItemCommand(Guid ProductId, UpdateCartItemDto CartItemDto) : ICommand<CartItemDto>;