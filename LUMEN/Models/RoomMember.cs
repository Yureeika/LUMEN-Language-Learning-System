namespace LUMEN.Models;

using System.ComponentModel.DataAnnotations;

public class RoomMember
{
    [Key]
    public int Id { get; set; }

    public int ChatRoomId { get; set; }

    [Required]
    public required string UserId { get; set; }

    public DateTime JoinedAt { get; set; } = DateTime.UtcNow;

    public virtual ChatRoom ChatRoom { get; set; } = null!;
    public virtual ApplicationUser User { get; set; } = null!;
}