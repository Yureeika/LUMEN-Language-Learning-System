namespace LUMEN.Models;

using System.ComponentModel.DataAnnotations;

public class OnlineUser
{
    [Key]
    public int Id { get; set; }

    [Required]
    public required string UserId { get; set; }

    [Required]
    public required string ConnectionId { get; set; }

    public DateTime LastSeen { get; set; } = DateTime.UtcNow;

    public virtual ApplicationUser User { get; set; } = null!;
}