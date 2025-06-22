using AutoMapper;
using Microsoft.AspNetCore.Http;
using Webapi.Application.Common.Exceptions;
using Webapi.Application.Common.Exceptions.CartItem;
using Webapi.Application.Common.Extensions;
using Webapi.Application.Common.Interfaces.MediatR;
using Webapi.Application.OrdersCQRS.Observers;
using Webapi.Domain.Entities;
using Webapi.Domain.Interfaces;
using Webapi.Domain.ValueObjects;
using Webapi.SharedKernel.DTOs.Orders;

namespace Webapi.Application.OrdersCQRS.Commands.CreateOrder;

public class CreateOrderHandler : ICommandHandler<CreateOrderCommand, OrderDto>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly List<IOrderCreatedListener> _listeners = [];

    public CreateOrderHandler(
        IHttpContextAccessor httpContextAccessor,
        IUnitOfWork unitOfWork,
        IMapper mapper
    )
    {
        _httpContextAccessor = httpContextAccessor;
        _unitOfWork = unitOfWork;
        _mapper = mapper;

        // Subscribe to listeners
        Subscribe(new UpdateProductQuantityOrderCreatedListener(unitOfWork));
        Subscribe(new UpdateCartOrderCreatedListener(unitOfWork));
    }

    public async Task<OrderDto> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var userId = _httpContextAccessor.HttpContext!.User.GetUserId();

        List<CartItem> cartItems = [];
        foreach (var cartItemId in request.CreateOrderDto.CartItemIds)
        {
            var cartItem = await _unitOfWork.CartItemRepository.GetCartItemByIdAsync(cartItemId, cancellationToken)
                ?? throw new CartItemNotFoundException(cartItemId);

            if (cartItem.UserId != userId)
            {
                throw new ForbiddenAccessException("You do not have permission to access this cart item.");
            }

            cartItems.Add(cartItem);
        }

        // Calculate total price and shipping cost
        var totalPrice = cartItems.Sum(ci => ci.ProductSize.Product.Price * ci.Quantity);

        var orderId = Guid.NewGuid();
        var order = new Order
        {
            Id = orderId,
            OwnerId = userId,
            TotalPrice = totalPrice,
            ShippingType = request.CreateOrderDto.ShippingType,
            Address = new Address
            {
                ReceiverName = request.CreateOrderDto.ReceiverName,
                ReceiverEmail = request.CreateOrderDto.ReceiverEmail,
                DetailAddress = request.CreateOrderDto.DetailAddress
            },
            Products = [.. cartItems.Select(ci => new OrderProduct
            {
                OrderId = orderId,
                ProductSizeId = ci.ProductSizeId,
                Quantity = ci.Quantity,
            })]
        };

        _unitOfWork.OrderRepository.Add(order);

        if (!await _unitOfWork.CompleteAsync(cancellationToken))
        {
            throw new BadRequestException("Failed to add products to order.");
        }

        // Notify updating related
        await NotifyListenersAsync(cartItems, cancellationToken);

        order = await _unitOfWork.OrderRepository.GetOrderByIdAsync(orderId, cancellationToken)
            ?? throw new OrderNotFoundException(order.Id);

        return _mapper.Map<OrderDto>(order);
    }

    private void Subscribe(IOrderCreatedListener listener)
    {
        _listeners.Add(listener);
    }

    private async Task NotifyListenersAsync(List<CartItem> cartItemIds, CancellationToken cancellationToken)
    {
        foreach (var listener in _listeners)
        {
            await listener.UpdateAsync(cartItemIds, cancellationToken);
        }
    }
}
