namespace LUMEN.Models;

using System.ComponentModel.DataAnnotations;

public class Announcement
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(150)]
    public required string Title { get; set; }

    [Required]
    public required string Content { get; set; }

    [Required]
    public required string CreatedById { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? ExpiresAt { get; set; }
    public bool IsActive { get; set; } = true;

    public virtual ApplicationUser CreatedBy { get; set; } = null!;
}