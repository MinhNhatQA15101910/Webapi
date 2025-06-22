using AutoMapper;
using Microsoft.AspNetCore.Http;
using Webapi.Application.Common.Exceptions;
using Webapi.Application.Common.Exceptions.CartItem;
using Webapi.Application.Common.Extensions;
using Webapi.Application.Common.Interfaces.MediatR;
using Webapi.Domain.Interfaces;
using Webapi.SharedKernel.DTOs.CartItem;

namespace Webapi.Application.CartItemCQRS.Commands.UpdateCartItem;

public class UpdateCartItemHandler(
    IHttpContextAccessor httpContextAccessor,
    IUnitOfWork unitOfWork,
    IMapper mapper
) : ICommandHandler<UpdateCartItemCommand, CartItemDto>
{
    public async Task<CartItemDto> Handle(UpdateCartItemCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var userId = httpContextAccessor.HttpContext!.User.GetUserId();
            
            // Get cart item by its ID instead of product ID
            var cartItem = await unitOfWork.CartItemRepository.GetCartItemByIdAsync(request.CartItemId, cancellationToken)
                ?? throw new CartItemNotFoundException(request.CartItemId);
                
            // Verify the cart item belongs to the current user
            if (cartItem.UserId != userId)
            {
                throw new BadRequestException("You can only update items in your own cart");
            }
            
            // Check if the product size is in stock
            var productSize = await unitOfWork.ProductSizeRepository.GetByIdAsync(cartItem.ProductSizeId, cancellationToken);
            if (productSize == null || productSize.Quantity < request.CartItemDto.Quantity)
            {
                throw new CartItemUpdateException(request.CartItemId, "Product is not available in the requested quantity");
            }
            
            // Update quantity
            cartItem.Quantity = request.CartItemDto.Quantity;
            cartItem.UpdatedAt = DateTime.UtcNow;
            
            unitOfWork.CartItemRepository.Update(cartItem);
            await unitOfWork.CompleteAsync(cancellationToken);
            
            // Get the updated cart item with product details
            var updatedCartItem = await unitOfWork.CartItemRepository.GetCartItemByIdAsync(request.CartItemId, cancellationToken)
                ?? throw new CartItemNotFoundException(request.CartItemId);
            
            return mapper.Map<CartItemDto>(updatedCartItem);
        }
        catch (CartItemNotFoundException)
        {
            // Rethrow specific exceptions
            throw;
        }
        catch (BadRequestException)
        {
            // Rethrow specific exceptions
            throw;
        }
        catch (Exception ex)
        {
            // Wrap other exceptions
            throw new CartItemUpdateException(request.CartItemId, $"An unexpected error occurred: {ex.Message}");
        }
    }
}