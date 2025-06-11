using Webapi.Application.Common.Interfaces.MediatR;

namespace Webapi.Application.CartItemCQRS.Commands.ClearCart;

public record ClearCartCommand() : ICommand<bool>;