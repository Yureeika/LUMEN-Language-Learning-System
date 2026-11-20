namespace LUMEN.Models;

using System.ComponentModel.DataAnnotations;

public class Lesson
{
    [Key]
    public int Id { get; set; }

    public int SubjectId { get; set; }

    [Required]
    [StringLength(150)]
    public string Title { get; set; }

    public string? Content { get; set; }
    public string? VideoUrl { get; set; }
    public int OrderIndex { get; set; }
    public bool IsPublished { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public virtual Subject? Subject { get; set; }
    public virtual ICollection<Quiz> Quizzes { get; } = [];
    public virtual ICollection<Resource> Resources { get; } = [];
    public virtual ICollection<VideoProgress> VideoProgresses { get; } = [];
}
