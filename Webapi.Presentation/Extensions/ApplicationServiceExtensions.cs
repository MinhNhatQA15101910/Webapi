using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Webapi.Application;
using Webapi.Application.Common.Helpers;
using Webapi.Application.Common.Interfaces.Factories;
using Webapi.Application.Common.Interfaces.Services;
using Webapi.Domain.Factories;
using Webapi.Domain.Interfaces;
using Webapi.Infrastructure.Persistence;
using Webapi.Infrastructure.Persistence.Proxies;
using Webapi.Infrastructure.Persistence.Repositories;
using Webapi.Infrastructure.Repositories;
using Webapi.Infrastructure.Services.Adapters;
using Webapi.Infrastructure.Services.Configurations;
using Webapi.Infrastructure.Services.Factories;
using Webapi.Infrastructure.Services.Services;
using Webapi.Infrastructure.Services.Services.Payment;
using Webapi.Presentation.Middlewares;

namespace Webapi.Presentation.Extensions;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddControllers();
        services.AddMemoryCache();
        services.AddHttpContextAccessor();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        // Add CORS services
        services.AddCors();

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
        services.Configure<CloudinarySettings>(config.GetSection(nameof(CloudinarySettings)));
        services.Configure<CacheSettings>(config.GetSection(nameof(CacheSettings)));
        services.Configure<MomoSettings>(config.GetSection(nameof(MomoSettings)));
        services.Configure<VNPaySettings>(config.GetSection(nameof(VNPaySettings)));

        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IFileService, FileService>();
        services.AddScoped<ICacheService, CacheService>();
        services.AddScoped<IPaymentService, PaymentService>();


        services.AddScoped<VNPayPaymentStrategy>();
        services.AddScoped<MomoPaymentStrategy>();

        // Register voucher import services
        services.AddScoped<JsonVoucherImport>();
        services.AddScoped<XlsVoucherImport>();
        services.AddScoped<XlsVoucherImportAdapter>();

        services.AddScoped<IVoucherImportFactory, VoucherImportFactory>();
        services.AddScoped<VoucherFactory>();

        return services;
    }

    private static IServiceCollection AddDatabaseContext(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlite(config.GetConnectionString("DefaultConnection"));
        });
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<UserRepository>();
        services.AddScoped<OrderRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IProductPhotoRepository, ProductPhotoRepository>();
        services.AddScoped<IProductSizeRepository, ProductSizeRepository>();
        services.AddScoped<ICartItemRepository, CartItemRepository>();
        services.AddScoped<IVoucherRepository, VoucherRepository>();
        services.AddScoped<IVoucherItemRepository, VoucherItemRepository>();
        services.AddScoped<IReviewRepository, ReviewRepository>(); // Add this line

        services.AddScoped<IUserRepository, UserProxy>();
        services.AddScoped<IOrderRepository, OrderProxy>();

        return services;
    }
}
