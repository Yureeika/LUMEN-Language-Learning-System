namespace LUMEN.Models;

using System.ComponentModel.DataAnnotations;

public class AuditLog
{
    [Key]
    public int Id { get; set; }

    [Required]
    public required string UserId { get; set; }

    [Required]
    [StringLength(100)]
    public required string Action { get; set; }

    [StringLength(255)]
    public string? EntityName { get; set; }

    public int? EntityId { get; set; }
    public string? OldValues { get; set; }
    public string? NewValues { get; set; }
    public string? IpAddress { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public virtual ApplicationUser User { get; set; } = null!;
}