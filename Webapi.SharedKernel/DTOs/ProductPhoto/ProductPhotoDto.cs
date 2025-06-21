namespace Webapi.SharedKernel.DTOs.ProductPhoto;

public class ProductPhotoDto
{
    public Guid Id { get; set; }
    public string Url { get; set; } = string.Empty;
    public bool IsMain { get; set; }
    public string PublicId { get; set; } = string.Empty;
}