namespace LUMEN.Models;

using System.ComponentModel.DataAnnotations;

public class Question
{
    [Key]
    public int Id { get; set; }

    public int QuizId { get; set; }

    [Required]
    public required string QuestionText { get; set; }

    [Required]
    public required string OptionA { get; set; }

    [Required]
    public required string OptionB { get; set; }

    [Required]
    public required string OptionC { get; set; }

    [Required]
    public required string OptionD { get; set; }

    [Required]
    [StringLength(1)]
    public required string CorrectAnswer { get; set; }

    // Points awarded when the learner answers correctly.
    // Default is 1 point.
    [Range(1, 100)]
    public int Points { get; set; } = 1;

    public virtual Quiz Quiz { get; set; } = null!;
}