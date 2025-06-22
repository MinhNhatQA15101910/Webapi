using AutoMapper;
using Microsoft.AspNetCore.Http;
using Webapi.Application.Common.Exceptions;
using Webapi.Application.Common.Exceptions.CartItem;
using Webapi.Application.Common.Extensions;
using Webapi.Application.Common.Interfaces.MediatR;
using Webapi.Application.Common.Interfaces.Services;
using Webapi.Application.OrdersCQRS.Observers.OrderCreated;
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
        IEmailService emailService,
        IMapper mapper
    )
    {
        _httpContextAccessor = httpContextAccessor;
        _unitOfWork = unitOfWork;
        _mapper = mapper;

        // Subscribe to listeners
        Subscribe(new UpdateProductQuantityOrderCreatedListener(unitOfWork));
        Subscribe(new UpdateCartOrderCreatedListener(unitOfWork));
        Subscribe(new UpdateVoucherQuantityOrderCreatedListener(unitOfWork));
        Subscribe(new SendEmailOrderCreatedListener(httpContextAccessor, unitOfWork, emailService));
    }

    public async Task<OrderDto> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var httpContext = _httpContextAccessor.HttpContext
            ?? throw new UnauthorizedAccessException("No HttpContext found.");

        var userId = httpContext.User.GetUserId();

        var cartItemTasks = request.CreateOrderDto.CartItemIds
            .Select(id => _unitOfWork.CartItemRepository.GetCartItemByIdAsync(id, cancellationToken))
            .ToList();

        var cartItems = new List<CartItem>();
        for (int i = 0; i < cartItemTasks.Count; i++)
        {
            var cartItem = await cartItemTasks[i] ??
                throw new CartItemNotFoundException(request.CreateOrderDto.CartItemIds[i]);

            if (cartItem.UserId != userId)
                throw new ForbiddenAccessException("You do not have permission to access this cart item.");

            cartItems.Add(cartItem);
        }

        // 2. Calculate total price
        var totalPrice = cartItems.Sum(ci => ci.ProductSize.Product.Price * ci.Quantity);
        var vouchers = new List<Voucher>();
        foreach (var voucherId in request.CreateOrderDto.VoucherIds)
        {
            var voucher = await _unitOfWork.VoucherRepository.GetByIdAsync(voucherId, cancellationToken)
                ?? throw new VoucherNotFoundException(voucherId);

            if (voucher.Quantity <= 0)
                throw new BadRequestException($"Voucher {voucher.Type.Name} is out of stock.");

            if (voucher.ExpiredAt < DateTime.UtcNow)
                throw new BadRequestException($"Voucher {voucher.Type.Name} has expired.");

            totalPrice -= totalPrice * (decimal)(voucher.Type.Value / 100);

            vouchers.Add(voucher);
        }

        // 3. Create order with address and products
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
            })],
            Vouchers = [.. request.CreateOrderDto.VoucherIds.Select(voucherId => new OrderVoucher
            {
                OrderId = orderId,
                VoucherId = voucherId
            })]
        };

        _unitOfWork.OrderRepository.Add(order);

        if (!await _unitOfWork.CompleteAsync(cancellationToken))
            throw new BadRequestException("Failed to complete order creation.");

        await NotifyListenersAsync(order, cartItems, vouchers, cancellationToken);

        var savedOrder = await _unitOfWork.OrderRepository.GetOrderByIdAsync(orderId, cancellationToken)
            ?? throw new OrderNotFoundException(orderId);

        return _mapper.Map<OrderDto>(savedOrder);
    }

    private void Subscribe(IOrderCreatedListener listener)
    {
        _listeners.Add(listener);
    }

    private async Task NotifyListenersAsync(Order order, List<CartItem> cartItemIds, List<Voucher> vouchers, CancellationToken cancellationToken)
    {
        foreach (var listener in _listeners)
        {
            await listener.UpdateAsync(order, cartItemIds, vouchers, cancellationToken);
        }
    }
}
