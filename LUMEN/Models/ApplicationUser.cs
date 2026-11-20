namespace LUMEN.Models;

using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public string? Nickname { get; set; }

    // Keep Category for backward compatibility
    public string Category { get; set; } = "";

    // New: Multiple categories (comma-separated)
    public string Categories { get; set; } = "";

    public int TotalXP { get; set; }
    public string? CurrentLevel { get; set; }
    public DateTime? LastLoginAt { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsSoftDeleted { get; set; }
}