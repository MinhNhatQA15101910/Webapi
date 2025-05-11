using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Webapi.Application;
using Webapi.Application.Common.Helpers;
using Webapi.Application.Common.Interfaces.Services;
using Webapi.Domain.Interfaces;
using Webapi.Infrastructure.Persistence;
using Webapi.Infrastructure.Persistence.Repositories;
using Webapi.Infrastructure.Services.Configurations;
using Webapi.Infrastructure.Services.Services;
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
            .AddApplication()
            .AddExternalServices(config);
    }

    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var applicationAssembly = typeof(AssemblyReference).Assembly;
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(applicationAssembly));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddValidatorsFromAssembly(applicationAssembly);

        services.AddSingleton<PincodeStore>();

        services.AddAutoMapper(applicationAssembly);

        return services;
    }

    public static IServiceCollection AddExternalServices(this IServiceCollection services, IConfiguration config)
    {
        services.Configure<EmailSenderSettings>(config.GetSection(nameof(EmailSenderSettings)));

        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IEmailService, EmailService>();

        return services;
    }

    private static IServiceCollection AddDatabaseContext(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlite(config.GetConnectionString("DefaultConnection"));
        });
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IUserRepository, UserRepository>();

        return services;
    }
}
