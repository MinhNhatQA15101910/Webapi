using AutoMapper;
using Microsoft.AspNetCore.Http;
using Webapi.Application.Common.Exceptions.CartItem;
using Webapi.Application.Common.Exceptions.Product;
using Webapi.Application.Common.Extensions;
using Webapi.Application.Common.Interfaces.MediatR;
using Webapi.Domain.Entities;
using Webapi.Domain.Interfaces;
using Webapi.SharedKernel.DTOs.CartItem;

namespace Webapi.Application.CartItemCQRS.Commands.AddCartItem;

public class AddCartItemHandler(
    IHttpContextAccessor httpContextAccessor,
    IUnitOfWork unitOfWork,
    IMapper mapper
) : ICommandHandler<AddCartItemCommand, CartItemDto>
{
    public async Task<CartItemDto> Handle(AddCartItemCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var userId = httpContextAccessor.HttpContext.User.GetUserId();
            
            // Verify product exists
            var product = await unitOfWork.ProductRepository.GetByIdAsync(request.CartItemDto.ProductId, cancellationToken)
                ?? throw new ProductNotFoundException(request.CartItemDto.ProductId);
                
            // Check if the product is in stock
            if (!await unitOfWork.ProductRepository.IsInStockAsync(request.CartItemDto.ProductId, request.CartItemDto.Quantity, cancellationToken))
            {
                throw new CartItemCreateException($"Product {request.CartItemDto.ProductId} is not available in the requested quantity");
            }
            
            // Check if item already exists in cart
            var existingCartItem = await unitOfWork.CartItemRepository.GetCartItemAsync(userId, request.CartItemDto.ProductId, cancellationToken);
            
            if (existingCartItem != null)
            {
                // Update quantity instead of creating new
                existingCartItem.Quantity += request.CartItemDto.Quantity;
                existingCartItem.UpdatedAt = DateTime.UtcNow;
                
                unitOfWork.CartItemRepository.Update(existingCartItem);
                await unitOfWork.CompleteAsync();
                
                return mapper.Map<CartItemDto>(existingCartItem);
            }
            
            // Create new cart item
            var cartItem = new CartItem
            {
                UserId = userId,
                ProductId = request.CartItemDto.ProductId,
                Quantity = request.CartItemDto.Quantity,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            
            unitOfWork.CartItemRepository.Add(cartItem);
            await unitOfWork.CompleteAsync();
            
            // Get the cart item with product details
            var addedCartItem = await unitOfWork.CartItemRepository.GetCartItemWithDetailsAsync(userId, request.CartItemDto.ProductId, cancellationToken)
                ?? throw new CartItemNotFoundException(userId, request.CartItemDto.ProductId);
            
            return mapper.Map<CartItemDto>(addedCartItem);
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
        catch (CartItemCreateException)
        {
            // Rethrow specific exceptions
            throw;
        }
        catch (Exception ex)
        {
            // Wrap other exceptions
            throw new CartItemCreateException($"An unexpected error occurred: {ex.Message}");
        }
    }
}