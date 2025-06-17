using System.Text.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Webapi.Domain.Entities;
using Webapi.Domain.Interfaces;

namespace Webapi.Infrastructure.Persistence.Data;

public class Seed
{
    public static async Task SeedUsersAsync(
        UserManager<User> userManager,
        RoleManager<Role> roleManager
    )
    {
        if (await userManager.Users.AnyAsync()) return;

        var userData = await File.ReadAllTextAsync("../Webapi.Infrastructure.Persistence/Data/UserSeedData.json");

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        var users = JsonSerializer.Deserialize<List<User>>(userData, options);

        if (users == null) return;

        if (!await roleManager.Roles.AnyAsync())
        {
            var roles = new List<Role>
            {
                new() { Name = "User" },
                new() { Name = "Admin" },
            };

            foreach (var role in roles)
            {
                await roleManager.CreateAsync(role);
            }
        }

        int index = 0;
        foreach (var user in users)
        {
            Console.WriteLine(index.ToString());
            await userManager.CreateAsync(user, "Pa$$w0rd");

            if (index < 8)
            {
                await userManager.AddToRoleAsync(user, "User");
            }
            else
            {
                await userManager.AddToRoleAsync(user, "Admin");
            }

            index++;
        }
    }

    public static async Task SeedProductsAsync(
        IUnitOfWork unitOfWork
    )
    {
        if (await unitOfWork.ProductRepository.AnyAsync()) return;

        var productData = await File.ReadAllTextAsync("../Webapi.Infrastructure.Persistence/Data/ProductSeedData.json");

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        var products = JsonSerializer.Deserialize<List<Product>>(productData, options);

        if (products == null) return;

        foreach (var product in products)
        {
            Console.WriteLine($"Adding product: {product.Name}");

            unitOfWork.ProductRepository.Add(product);
        }
    }

    public static async Task SeedCategoriesAsync(
        IUnitOfWork unitOfWork
    )
    {
        if (await unitOfWork.CategoryRepository.AnyAsync()) return;

        var categoryData = await File.ReadAllTextAsync("../Webapi.Infrastructure.Persistence/Data/CategorySeedData.json");

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        var categories = JsonSerializer.Deserialize<List<Category>>(categoryData, options);

        if (categories == null) return;

        foreach (var category in categories)
        {
            Console.WriteLine($"Adding category: {category.Name}");

            unitOfWork.CategoryRepository.Add(category);
        }
    }

    public static async Task SeedProductCategoriesAsync(
        IUnitOfWork unitOfWork
    )
    {
        var productCategoriesData = await File.ReadAllTextAsync("../Webapi.Infrastructure.Persistence/Data/ProductCategorySeedData.json");

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        var productCategories = JsonSerializer.Deserialize<List<ProductCategory>>(productCategoriesData, options);

        if (productCategories == null) return;

        foreach (var productCategory in productCategories)
        {
            var product = await unitOfWork.ProductRepository.GetByIdAsync(productCategory.ProductId);
            if (product == null)
            {
                Console.WriteLine($"Product with ID {productCategory.ProductId} not found. Skipping.");
                continue;
            }

            Console.WriteLine($"Adding product category: {productCategory.CategoryId} to product: {product.Name}");

            product.Categories.Add(productCategory);
        }
    }
}
