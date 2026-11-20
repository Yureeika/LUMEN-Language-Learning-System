namespace LUMEN.Models;

using System.ComponentModel.DataAnnotations;

public class Bookmark
{
    [Key]
    public int Id { get; set; }

    [Required]
    public required string UserId { get; set; }

    public int LessonId { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public virtual ApplicationUser User { get; set; } = null!;
    public virtual Lesson Lesson { get; set; } = null!;
}