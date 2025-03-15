using Application.Behaviors;
using Application.Helpers;
using Domain.Services;
using ExternalServices;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Web.Middlewares;

namespace Web.Extensions;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddControllers();

        // Database and repositories
        services.AddDbContext<DataContext>(options =>
        {
            options.UseSqlite(
                config.GetConnectionString("DefaultConnection"),
                options => options.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)
            );
        });

        // Services
        services.AddSingleton<PincodeStore>();
        services.AddScoped<ITokenService, TokenService>();

        // Middleware
        services.AddScoped<ExceptionHandlingMiddleware>();

        // MediatR
        var applicationAssembly = typeof(Application.AssemblyReference).Assembly;
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(applicationAssembly));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddValidatorsFromAssembly(applicationAssembly);

        // Others
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        return services;
    }
}
