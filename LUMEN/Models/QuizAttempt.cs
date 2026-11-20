namespace LUMEN.Models;

using System.ComponentModel.DataAnnotations;

public class QuizAttempt
{
    [Key]
    public int Id { get; set; }

    public int QuizId { get; set; }

    [Required]
    public required string UserId { get; set; }

    public int Score { get; set; }
    public int TotalPoints { get; set; }
    public bool Passed { get; set; }
    public DateTime StartedAt { get; set; } = DateTime.UtcNow;
    public DateTime? CompletedAt { get; set; }
    public string? AnswersJson { get; set; }

    public virtual Quiz Quiz { get; set; } = null!;
    public virtual ApplicationUser User { get; set; } = null!;
}