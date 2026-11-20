namespace LUMEN.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Post
{
    public int Id { get; set; }

    [Required]
    public string UserId { get; set; } = "";

    [ForeignKey("UserId")]
    public virtual ApplicationUser User { get; set; } = null!;

    public int ThreadId { get; set; }

    [ForeignKey("ThreadId")]
    public virtual DiscussionThread Thread { get; set; } = null!;

    public string Content { get; set; } = "";

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}