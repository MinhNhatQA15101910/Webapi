using AutoMapper;
using Microsoft.AspNetCore.Http;
using Webapi.Application.Common.Exceptions;
using Webapi.Application.Common.Exceptions.ProductSize;
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
        var userId = httpContextAccessor.HttpContext!.User.GetUserId();

        var productSize = await unitOfWork.ProductSizeRepository
            .GetByIdAsync(request.AddCartItemDto.ProductSizeId, cancellationToken)
            ?? throw new ProductSizeNotFoundException(request.AddCartItemDto.ProductSizeId);

        var cartItem = await unitOfWork.CartItemRepository.GetCartItemAsync(userId, productSize.Id, cancellationToken);
        if (cartItem != null)
        {
            cartItem.Quantity += request.AddCartItemDto.Quantity;
            cartItem.UpdatedAt = DateTime.UtcNow;
        }
        else
        {
            cartItem = new CartItem
            {
                UserId = userId,
                ProductSizeId = productSize.Id,
                Quantity = request.AddCartItemDto.Quantity
            };

            unitOfWork.CartItemRepository.Add(cartItem);
        }

        if (!await unitOfWork.CompleteAsync(cancellationToken))
        {
            throw new BadRequestException("Failed to add item to cart.");
        }

        return mapper.Map<CartItemDto>(cartItem);
    }
}
