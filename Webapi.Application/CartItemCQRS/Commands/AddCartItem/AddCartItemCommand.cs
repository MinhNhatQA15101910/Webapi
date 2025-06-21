using Webapi.Application.Common.Interfaces.MediatR;
using Webapi.SharedKernel.DTOs.CartItem;

namespace Webapi.Application.CartItemCQRS.Commands.AddCartItem;

public record AddCartItemCommand(AddCartItemDto AddCartItemDto) : ICommand<CartItemDto>;
