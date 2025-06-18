using Microsoft.EntityFrameworkCore;
using Webapi.Domain.Entities;
using Webapi.Domain.Interfaces;
using Webapi.SharedKernel.DTOs.Orders;
using Webapi.SharedKernel.Helpers;
using Webapi.SharedKernel.Params;

namespace Webapi.Infrastructure.Persistence.Repositories;

public class OrderRepository(AppDbContext dbContext) : IOrderRepository
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

    public Task<PagedList<OrderDto>> GetOrdersAsync(Guid? userId, OrderParams orderParams, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException("This method is not implemented yet.");
        // var query = dbContext.Orders.AsQueryable();

        // // Filter by userId if provided
        // if (userId != null)
        // {
        //     query = query.Where(o => o.OwnerId == userId);
        // }

        // // Filter by order status if provided
        // if (orderParams.OrderState != null)
        // {
        //     query = query.Where(o => o.OrderState == orderParams.OrderState);
        // }

        // // Filter by shipping type if provided
        // if (orderParams.ShippingType != null)
        // {
        //     query = query.Where(o => o.ShippingType == orderParams.ShippingType);
        // }

        // // Filter by price range
        // query = query.Where(o => o.TotalPrice >= orderParams.MinPrice && o.TotalPrice <= orderParams.MaxPrice);

        // // Filter by email
        // if (userParams.Email != null)
        // {
        //     query = query.Where(u => u.NormalizedEmail!.Contains(userParams.Email.ToUpper()));
        // }

        // // Order
        // query = userParams.OrderBy switch
        // {
        //     "email" => userParams.SortBy == "asc"
        //                 ? query.OrderBy(u => u.Email)
        //                 : query.OrderByDescending(u => u.Email),
        //     "updatedAt" => userParams.SortBy == "asc"
        //                 ? query.OrderBy(u => u.UpdatedAt)
        //                 : query.OrderByDescending(u => u.UpdatedAt),
        //     _ => query.OrderBy(u => u.Email)
        // };

        // return await PagedList<UserDto>.CreateAsync(
        //     query.ProjectTo<UserDto>(mapper.ConfigurationProvider),
        //     userParams.PageNumber,
        //     userParams.PageSize,
        //     cancellationToken
        // );
    }
}
