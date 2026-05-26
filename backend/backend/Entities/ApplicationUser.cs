using Microsoft.AspNetCore.Identity;

namespace backend.Entities;


public class ApplicationUser
{
    public Guid Id { get; set; }

    public string? FullName { get; set; }

    public string? Email { get; set; }

    public string? UserName { get; set; }

    public string? PasswordHash { get; set; }

    public string? PhoneNumber { get; set; }

    public string? ProfilePicture { get; set; }

    public string? GoogleId { get; set; }

    public string? Role { get; set; }

    public DateTime CreatedAt { get; set; }
}