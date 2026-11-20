namespace LUMEN.Models;

using System.ComponentModel.DataAnnotations;

public class Notification
{
    [Key]
    public int Id { get; set; }

    [Required]
    public required string UserId { get; set; }

    [Required]
    [StringLength(150)]
    public required string Title { get; set; }

    [Required]
    public required string Message { get; set; }

    public string? Link { get; set; }
    public bool IsRead { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public virtual ApplicationUser User { get; set; } = null!;
}