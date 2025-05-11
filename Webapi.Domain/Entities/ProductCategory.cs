using System.ComponentModel.DataAnnotations.Schema;

namespace Webapi.Domain.Entities;

[Table("ProductCategories")]
public class ProductCategory
{
    public Guid ProductId { get; set; }
    public Product Product { get; set; } = null!;
    public Guid CategoryId { get; set; }
    public Category Category { get; set; } = null!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
