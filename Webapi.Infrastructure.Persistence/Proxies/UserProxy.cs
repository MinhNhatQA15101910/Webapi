using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Webapi.Domain.Entities;
using Webapi.Domain.Interfaces;
using Webapi.Infrastructure.Persistence.Repositories;
using Webapi.Infrastructure.Services.Configurations;
using Webapi.SharedKernel.DTOs;
using Webapi.SharedKernel.Helpers;
using Webapi.SharedKernel.Params;

namespace Webapi.Infrastructure.Persistence.Proxies;

public class UserProxy(
    UserRepository userRepository,
    IOptions<CacheSettings> config,
    ICacheService cacheService
) : IUserRepository
{
    private readonly int _cacheAbsoluteDurationMinutes = config.Value.CacheAbsoluteDurationMinutes;
    private readonly int _cacheSlidingDurationMinutes = config.Value.CacheSlidingDurationMinutes;

    public async Task<User?> GetUserByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var cacheKey = $"Users_/{id}";

        if (!cacheService.TryGetValue(cacheKey, out User? user))
        {
            // Fetch countries from the database.
            user = await userRepository.GetUserByIdAsync(id, cancellationToken);

            // Set cache entry options with high priority.
            var cacheEntryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(_cacheAbsoluteDurationMinutes),
                SlidingExpiration = TimeSpan.FromMinutes(_cacheSlidingDurationMinutes)
            };

            cacheService.Set(cacheKey, user, cacheEntryOptions);
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

            // Set cache entry options with high priority.
            var cacheEntryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(_cacheAbsoluteDurationMinutes),
                SlidingExpiration = TimeSpan.FromMinutes(_cacheSlidingDurationMinutes)
            };

            cacheService.Set(cacheKey, users, cacheEntryOptions);
        }

        return users ?? new PagedList<UserDto>([], 0, userParams.PageNumber, userParams.PageSize);
    }

    private string GetUsersQueryCacheKey(Guid currentUserId, UserParams userParams)
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
