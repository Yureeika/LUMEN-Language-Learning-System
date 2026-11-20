namespace LUMEN.Models;

using System.ComponentModel.DataAnnotations;

public class UserBadge
{
    [Key]
    public int Id { get; set; }

    [Required]
    public required string UserId { get; set; }

    public int BadgeId { get; set; }

    public DateTime AwardedAt { get; set; } = DateTime.UtcNow;

    public virtual ApplicationUser User { get; set; } = null!;
    public virtual Badge Badge { get; set; } = null!;
}