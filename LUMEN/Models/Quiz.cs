namespace LUMEN.Models;

using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

public class Quiz
{
    [Key]
    public int Id { get; set; }

    public int LessonId { get; set; }

    [Required]
    [StringLength(150)]
    public required string Title { get; set; }

    public string? Description { get; set; }

    public int TimeLimitMinutes { get; set; } = 10;

    public int PassingScore { get; set; } = 70;

    public bool IsPublished { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [ValidateNever]
    public virtual Lesson Lesson { get; set; } = null!;

    public virtual ICollection<Question> Questions { get; } = [];

    public virtual ICollection<QuizAttempt> QuizAttempts { get; } = [];
}