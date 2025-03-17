using Application.DTOs.Photos;

namespace Application.DTOs.Users;

public class UserDto
{
    public Guid Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Token { get; set; }
    public string? PhotoUrl { get; set; }
    public List<PhotoDto> Photos { get; set; } = [];
    public List<string> Roles { get; set; } = [];
}
