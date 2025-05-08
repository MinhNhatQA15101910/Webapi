using Microsoft.EntityFrameworkCore;
using Webapi.Application;
using Webapi.Infrastructure.Persistence;
using Webapi.Presentation.Middlewares;

namespace Webapi.Presentation.Extensions;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddControllers();
        services.AddHttpContextAccessor();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        services.AddScoped<ExceptionHandlingMiddleware>();

        return services.AddDatabaseContext(config)
            .AddApplication();
    }

    private static IServiceCollection AddDatabaseContext(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlite(config.GetConnectionString("DefaultConnection"));
        });

        return services;
    }
}
