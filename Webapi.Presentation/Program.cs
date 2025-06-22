using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Webapi.Domain.Entities;
using Webapi.Domain.Interfaces;
using Webapi.Infrastructure.Persistence;
using Webapi.Infrastructure.Persistence.Data;
using Webapi.Presentation.Extensions;
using Webapi.Presentation.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddIdentityServices(builder.Configuration);

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Add CORS middleware
app.UseCors(policy => policy
    .AllowAnyHeader()
    .AllowAnyMethod()
    .WithOrigins(["http://localhost:3000", "http://localhost:3001"])
    .AllowCredentials());

app.MapControllers();

using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
try
{
    var context = services.GetRequiredService<AppDbContext>();
    var userManager = services.GetRequiredService<UserManager<User>>();
    var roleManager = services.GetRequiredService<RoleManager<Role>>();
    var unitOfWork = services.GetRequiredService<IUnitOfWork>();

    await context.Database.MigrateAsync();
    await Seed.SeedUsersAsync(userManager, roleManager);
    await Seed.SeedProductsAsync(unitOfWork);
    await Seed.SeedCategoriesAsync(unitOfWork);
    await Seed.SeedProductCategoriesAsync(unitOfWork);
    await Seed.SeedProductSizesAsync(unitOfWork);

    await unitOfWork.CompleteAsync();
}
catch (Exception ex)
{
    var logger = services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An error occurred during migration");
}

app.Run();
