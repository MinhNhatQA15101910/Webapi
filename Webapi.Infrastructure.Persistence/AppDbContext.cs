using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Webapi.Domain.Entities;

namespace Webapi.Infrastructure.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<
        User,
        Role,
        Guid,
        IdentityUserClaim<Guid>,
        UserRole,
        IdentityUserLogin<Guid>,
        IdentityRoleClaim<Guid>,
        IdentityUserToken<Guid>
    >(options)
{
    public DbSet<Product> Products { get; set; }
    public DbSet<CartItem> CartItems { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<Voucher> Vouchers { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<ProductPhoto> ProductPhotos { get; set; }
    public DbSet<ProductCategory> ProductCategories { get; set; }

    public DbSet<ProductSize> ProductSizes { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<User>()
            .HasMany(x => x.UserRoles)
            .WithOne(x => x.User)
            .HasForeignKey(x => x.UserId)
            .IsRequired();

        builder.Entity<Role>()
            .HasMany(x => x.UserRoles)
            .WithOne(x => x.Role)
            .HasForeignKey(x => x.RoleId)
            .IsRequired();

        builder.Entity<Order>()
            .OwnsOne(x => x.Address);

        builder.Entity<OrderProduct>()
            .HasKey(x => new { x.OrderId, x.ProductSizeId });

        builder.Entity<ProductSize>()
            .HasMany(x => x.Orders)
            .WithOne(x => x.ProductSize)
            .HasForeignKey(x => x.ProductSizeId)
            .IsRequired();

        builder.Entity<Order>()
            .HasMany(x => x.Products)
            .WithOne(x => x.Order)
            .HasForeignKey(x => x.OrderId)
            .IsRequired();

        builder.Entity<OrderVoucher>()
            .HasKey(x => new { x.OrderId, x.VoucherId });

        builder.Entity<Voucher>()
            .HasMany(x => x.Orders)
            .WithOne(x => x.Voucher)
            .HasForeignKey(x => x.VoucherId)
            .IsRequired();

        builder.Entity<Order>()
            .HasMany(x => x.Vouchers)
            .WithOne(x => x.Order)
            .HasForeignKey(x => x.OrderId)
            .IsRequired();

        builder.Entity<ProductCategory>()
            .HasKey(x => new { x.ProductId, x.CategoryId });

        builder.Entity<Product>()
            .HasMany(x => x.Categories)
            .WithOne(x => x.Product)
            .HasForeignKey(x => x.ProductId)
            .IsRequired();

        builder.Entity<Category>()
            .HasMany(x => x.Products)
            .WithOne(x => x.Category)
            .HasForeignKey(x => x.CategoryId)
            .IsRequired();
    }
}
