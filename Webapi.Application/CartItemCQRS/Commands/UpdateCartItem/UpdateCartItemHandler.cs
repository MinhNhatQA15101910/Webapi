using AutoMapper;
using Microsoft.AspNetCore.Http;
using Webapi.Application.Common.Exceptions.CartItem;
using Webapi.Application.Common.Exceptions.Product;
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
            var userId = httpContextAccessor.HttpContext.User.GetUserId();
            
            // Get cart item
            var cartItem = await unitOfWork.CartItemRepository.GetCartItemAsync(userId, request.ProductId, cancellationToken)
                ?? throw new CartItemNotFoundException(userId, request.ProductId);
                
            // Verify product exists
            var product = await unitOfWork.ProductRepository.GetByIdAsync(request.ProductId, cancellationToken)
                ?? throw new ProductNotFoundException(request.ProductId);
                
            // Check if the product is in stock
            if (!await unitOfWork.ProductRepository.IsInStockAsync(request.ProductId, request.CartItemDto.Quantity, cancellationToken))
            {
                throw new CartItemUpdateException(request.ProductId, "Product is not available in the requested quantity");
            }
            
            // Update quantity
            cartItem.Quantity = request.CartItemDto.Quantity;
            cartItem.UpdatedAt = DateTime.UtcNow;
            
            unitOfWork.CartItemRepository.Update(cartItem);
            await unitOfWork.CompleteAsync();
            
            // Get the updated cart item with product details
            var updatedCartItem = await unitOfWork.CartItemRepository.GetCartItemWithDetailsAsync(userId, request.ProductId, cancellationToken)
                ?? throw new CartItemNotFoundException(userId, request.ProductId);
            
            return mapper.Map<CartItemDto>(updatedCartItem);
        }
        catch (ProductNotFoundException)
        {
            // Rethrow specific exceptions
            throw;
        }
        catch (CartItemNotFoundException)
        {
            // Rethrow specific exceptions
            throw;
        }
        catch (CartItemUpdateException)
        {
            // Rethrow specific exceptions
            throw;
        }
        catch (Exception ex)
        {
            // Wrap other exceptions
            throw new CartItemUpdateException(request.ProductId, $"An unexpected error occurred: {ex.Message}");
        }
    }
}