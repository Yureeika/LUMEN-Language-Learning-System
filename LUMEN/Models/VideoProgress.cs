namespace LUMEN.Models;

using System.ComponentModel.DataAnnotations;

public class VideoProgress
{
    [Key]
    public int Id { get; set; }

    [Required]
    public required string UserId { get; set; }

    public int LessonId { get; set; }

    public int ProgressSeconds { get; set; }
    public bool IsCompleted { get; set; } = false;
    public DateTime LastUpdated { get; set; } = DateTime.UtcNow;

    public virtual ApplicationUser User { get; set; } = null!;
    public virtual Lesson Lesson { get; set; } = null!;
}