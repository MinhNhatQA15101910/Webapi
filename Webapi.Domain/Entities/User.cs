using Microsoft.AspNetCore.Identity;

namespace Webapi.Domain.Entities;

public class User : IdentityUser<Guid>
{
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public ICollection<UserPhoto> Photos { get; set; } = [];
    public ICollection<UserRole> UserRoles { get; set; } = [];
    public ICollection<CartItem> CartItems { get; set; } = [];
    public ICollection<Order> Orders { get; set; } = [];
}
