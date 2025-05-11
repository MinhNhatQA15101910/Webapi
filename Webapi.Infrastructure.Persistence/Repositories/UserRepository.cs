using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Webapi.Domain.Entities;
using Webapi.Domain.Interfaces;
using Webapi.SharedKernel.DTOs;
using Webapi.SharedKernel.Helpers;
using Webapi.SharedKernel.Params;

namespace Webapi.Infrastructure.Persistence.Repositories;

public class UserRepository(
    AppDbContext dbContext,
    IMapper mapper
) : IUserRepository
{
    public async Task<User?> GetUserByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await dbContext.Users
            .Include(u => u.UserRoles).ThenInclude(ur => ur.Role)
            .Include(u => u.Photos)
            .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
    }

    public async Task<PagedList<UserDto>> GetUsersAsync(Guid currentUserId, UserParams userParams, CancellationToken cancellationToken = default)
    {
        var query = dbContext.Users.AsQueryable();

        // Remove current user
        query = query.Where(u => u.Id != currentUserId);

        // Filter by email
        if (userParams.Email != null)
        {
            query = query.Where(u => u.NormalizedEmail!.Contains(userParams.Email.ToUpper()));
        }

        // Order
        query = userParams.OrderBy switch
        {
            "email" => userParams.SortBy == "asc"
                        ? query.OrderBy(u => u.Email)
                        : query.OrderByDescending(u => u.Email),
            "updatedAt" => userParams.SortBy == "asc"
                        ? query.OrderBy(u => u.UpdatedAt)
                        : query.OrderByDescending(u => u.UpdatedAt),
            _ => query.OrderBy(u => u.Email)
        };

        return await PagedList<UserDto>.CreateAsync(
            query.ProjectTo<UserDto>(mapper.ConfigurationProvider),
            userParams.PageNumber,
            userParams.PageSize,
            cancellationToken
        );
    }
}
