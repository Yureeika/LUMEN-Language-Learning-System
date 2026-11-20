namespace LUMEN.Models;

using System.ComponentModel.DataAnnotations;

public class ChatMessage
{
    [Key]
    public int Id { get; set; }

    public int ChatRoomId { get; set; }

    [Required]
    public required string UserId { get; set; }

    [Required]
    public required string Content { get; set; }

    public DateTime SentAt { get; set; } = DateTime.UtcNow;
    public bool IsEdited { get; set; } = false;

    public virtual ChatRoom ChatRoom { get; set; } = null!;
    public virtual ApplicationUser User { get; set; } = null!;
}