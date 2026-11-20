namespace LUMEN.Models;

using System.ComponentModel.DataAnnotations;

public class Badge
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public required string Name { get; set; }

    [StringLength(255)]
    public string? Description { get; set; }

    [StringLength(255)]
    public string? IconUrl { get; set; }

    public int RequiredXP { get; set; }
    public bool IsActive { get; set; } = true;

    public virtual ICollection<UserBadge> UserBadges { get; } = [];
}