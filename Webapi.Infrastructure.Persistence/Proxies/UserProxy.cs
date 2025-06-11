using Webapi.Domain.Entities;
using Webapi.Domain.Interfaces;
using Webapi.Infrastructure.Persistence.Repositories;
using Webapi.SharedKernel.DTOs;
using Webapi.SharedKernel.Helpers;
using Webapi.SharedKernel.Params;

namespace Webapi.Infrastructure.Persistence.Proxies;

public class UserProxy(
    UserRepository userRepository,
    ICacheService cacheService
) : IUserRepository
{
    public async Task<User?> GetUserByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var cacheKey = $"Users_/{id}";

        if (!cacheService.TryGetValue(cacheKey, out User? user))
        {
            // Fetch countries from the database.
            user = await userRepository.GetUserByIdAsync(id, cancellationToken);

            cacheService.Set(cacheKey, user);
        }

        return user ?? new User();
    }

    public async Task<PagedList<UserDto>> GetUsersAsync(Guid currentUserId, UserParams userParams, CancellationToken cancellationToken = default)
    {
        var cacheKey = GetUsersQueryCacheKey(currentUserId, userParams);

        if (!cacheService.TryGetValue(cacheKey, out PagedList<UserDto>? users))
        {
            // Fetch countries from the database.
            users = await userRepository.GetUsersAsync(currentUserId, userParams, cancellationToken);

            cacheService.Set(cacheKey, users);
        }

        return users ?? new PagedList<UserDto>([], 0, userParams.PageNumber, userParams.PageSize);
    }

    private static string GetUsersQueryCacheKey(Guid currentUserId, UserParams userParams)
    {
        var cacheKey = $"Users_";
        cacheKey += $"?pageNumber={userParams.PageNumber}";
        cacheKey += $"&pageSize={userParams.PageSize}";
        cacheKey += $"&currentUserId={currentUserId}";
        if (userParams.Email != null)
        {
            cacheKey += $"&email={userParams.Email}";
        }
        cacheKey += $"&orderBy={userParams.OrderBy}";
        cacheKey += $"&sortBy={userParams.SortBy}";
        return cacheKey;
    }
}
