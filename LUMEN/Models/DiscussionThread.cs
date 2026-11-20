namespace LUMEN.Models;

using System.ComponentModel.DataAnnotations;

public class DiscussionThread
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(150)]
    public required string Title { get; set; }

    [Required]
    public required string UserId { get; set; }

    public int? SubjectId { get; set; }
    public int? LessonId { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsLocked { get; set; } = false;

    public virtual ApplicationUser User { get; set; } = null!;
    public virtual Subject? Subject { get; set; }
    public virtual Lesson? Lesson { get; set; }
    public virtual ICollection<Post> Posts { get; } = [];
}