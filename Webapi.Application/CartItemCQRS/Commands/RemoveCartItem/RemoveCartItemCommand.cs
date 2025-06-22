using Webapi.Application.Common.Interfaces.MediatR;
using Webapi.SharedKernel.DTOs.CartItem;

namespace Webapi.Application.CartItemCQRS.Commands.RemoveCartItem;

public record RemoveCartItemCommand(Guid CartItemId) : ICommand<CartItemDto>;