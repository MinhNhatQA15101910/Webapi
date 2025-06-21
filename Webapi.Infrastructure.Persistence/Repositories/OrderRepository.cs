using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Webapi.Domain.Entities;
using Webapi.Domain.Interfaces;
using Webapi.SharedKernel.DTOs.Orders;
using Webapi.SharedKernel.Helpers;
using Webapi.SharedKernel.Params;

namespace Webapi.Infrastructure.Persistence.Repositories;

public class OrderRepository(
    AppDbContext dbContext,
    IMapper mapper
) : IOrderRepository
{
    public void Add(Order order)
    {
        dbContext.Orders.Add(order);
    }

    public async Task<Order?> GetOrderByIdAsync(Guid orderId, CancellationToken cancellationToken)
    {
        return await dbContext.Orders
            .Include(o => o.Products)
                .ThenInclude(op => op.ProductSize)
                    .ThenInclude(ps => ps.Product)
                        .ThenInclude(p => p.Photos)
            .FirstOrDefaultAsync(o => o.Id == orderId, cancellationToken);
    }

    public async Task<PagedList<OrderDto>> GetOrdersAsync(Guid? userId, OrderParams orderParams, CancellationToken cancellationToken = default)
    {
        var query = dbContext.Orders.AsQueryable();

        // Filter by userId if provided
        if (userId != null)
        {
            query = query.Where(o => o.OwnerId == userId);
        }

        // Filter by order status if provided
        if (orderParams.OrderState != null)
        {
            query = query.Where(o => o.OrderState == orderParams.OrderState);
        }

        // Filter by shipping type if provided
        if (orderParams.ShippingType != null)
        {
            query = query.Where(o => o.ShippingType == orderParams.ShippingType);
        }

        // Filter by price range
        query = query.Where(o => o.TotalPrice >= orderParams.MinPrice && o.TotalPrice <= orderParams.MaxPrice);

        // Order
        query = orderParams.OrderBy switch
        {
            "createdAt" => orderParams.SortBy == "desc"
                ? query.OrderByDescending(o => o.CreatedAt)
                : query.OrderBy(o => o.CreatedAt),
            "totalPrice" => orderParams.SortBy == "desc"
                ? query.OrderByDescending(o => o.TotalPrice)
                : query.OrderBy(o => o.TotalPrice),
            "shippingType" => orderParams.SortBy == "desc"
                ? query.OrderByDescending(o => o.ShippingType)
                : query.OrderBy(o => o.ShippingType),
            _ => query.OrderBy(o => o.CreatedAt)
        };

        return await PagedList<OrderDto>.CreateAsync(
            query.ProjectTo<OrderDto>(mapper.ConfigurationProvider),
            orderParams.PageNumber,
            orderParams.PageSize,
            cancellationToken
        );
    }

    public void Update(Order order)
    {
        order.UpdatedAt = DateTime.UtcNow;
        dbContext.Orders.Update(order);
    }
}
