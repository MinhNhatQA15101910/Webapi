using System.ComponentModel.DataAnnotations.Schema;

namespace Webapi.Domain.Entities;

[Table("ProductPhotos")]
public class ProductPhoto
{
    public Guid Id { get; set; }
    public required string Url { get; set; }
    public string? PublicId { get; set; }
    public bool IsMain { get; set; }

    // Navigation properties
    public Guid ProductId { get; set; }
    public Product Product { get; set; } = null!;
    
    // Optional: If photos are managed by users (e.g., uploaded by admin)
    public Guid? UserId { get; set; }
    public User? User { get; set; }
}
