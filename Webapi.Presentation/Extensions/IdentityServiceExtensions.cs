using Microsoft.AspNetCore.Identity;
using Webapi.Domain.Entities;
using Webapi.Infrastructure.Persistence;

namespace Webapi.Presentation.Extensions;

public static class IdentityServiceExtensions
{
    public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddIdentityCore<User>(options =>
        {
            options.Password.RequireNonAlphanumeric = false;
        })
            .AddRoles<Role>()
            .AddRoleManager<RoleManager<Role>>()
            .AddEntityFrameworkStores<AppDbContext>();

        return services;
    }
}
