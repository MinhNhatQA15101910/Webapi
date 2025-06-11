using AutoMapper;
using Microsoft.AspNetCore.Http;
using Webapi.Application.Common.Exceptions.CartItem;
using Webapi.Application.Common.Extensions;
using Webapi.Application.Common.Interfaces.MediatR;
using Webapi.Domain.Interfaces;
using Webapi.SharedKernel.DTOs.CartItem;

namespace Webapi.Application.CartItemCQRS.Commands.RemoveCartItem;

public class RemoveCartItemHandler(
    IHttpContextAccessor httpContextAccessor,
    IUnitOfWork unitOfWork,
    IMapper mapper
) : ICommandHandler<RemoveCartItemCommand, CartItemDto>
{
    public async Task<CartItemDto> Handle(RemoveCartItemCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var userId = httpContextAccessor.HttpContext.User.GetUserId();
            
            // Get cart item with details for return value
            var cartItem = await unitOfWork.CartItemRepository.GetCartItemWithDetailsAsync(userId, request.ProductId, cancellationToken)
                ?? throw new CartItemNotFoundException(userId, request.ProductId);
                
            // Map to DTO for return value
            var cartItemDto = mapper.Map<CartItemDto>(cartItem);
            
            // Remove cart item
            unitOfWork.CartItemRepository.Remove(cartItem);
            await unitOfWork.CompleteAsync();
            
            return cartItemDto;
        }
        catch (CartItemNotFoundException)
        {
            // Rethrow specific exceptions
            throw;
        }
        catch (Exception ex)
        {
            // Wrap other exceptions
            throw new CartItemDeleteException(request.ProductId, $"An unexpected error occurred: {ex.Message}");
        }
    }
}