namespace LUMEN.Models;

using System.ComponentModel.DataAnnotations;

public class ChatRoom
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(150)]
    public required string Name { get; set; }

    public string? Description { get; set; }
    public bool IsPublic { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public virtual ICollection<RoomMember> Members { get; } = [];
    public virtual ICollection<ChatMessage> Messages { get; } = [];
}