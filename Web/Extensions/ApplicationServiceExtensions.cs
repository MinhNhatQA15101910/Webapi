using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Web.Extensions;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddControllers();
        services.AddDbContext<DataContext>(
            options => options.UseSqlite(
                config.GetConnectionString("DefaultConnection")
            )
        );

        return services;
    }
}
