using Microsoft.AspNetCore.Http;
using Webapi.Application.Common.Exceptions.CartItem;
using Webapi.Application.Common.Extensions;
using Webapi.Application.Common.Interfaces.MediatR;
using Webapi.Domain.Interfaces;

namespace Webapi.Application.CartItemCQRS.Commands.ClearCart;

public class ClearCartHandler(
    IHttpContextAccessor httpContextAccessor,
    IUnitOfWork unitOfWork
) : ICommandHandler<ClearCartCommand, bool>
{
    public async Task<bool> Handle(ClearCartCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var userId = httpContextAccessor.HttpContext.User.GetUserId();
            
            // Clear all cart items for the user
            await unitOfWork.CartItemRepository.ClearCartAsync(userId, cancellationToken);
            await unitOfWork.CompleteAsync();
            
            return true;
        }
        catch (Exception ex)
        {
            // Wrap other exceptions
            throw new CartItemDeleteException(Guid.Empty, $"An unexpected error occurred while clearing cart: {ex.Message}");
        }
    }
}