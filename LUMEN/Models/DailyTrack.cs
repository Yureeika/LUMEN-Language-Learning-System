namespace LUMEN.Models;

using System.ComponentModel.DataAnnotations;

public class DailyTrack
{
    [Key]
    public int Id { get; set; }

    [Required]
    public required string UserId { get; set; }

    public DateOnly TrackDate { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);
    public int XPEarned { get; set; }
    public int LessonsCompleted { get; set; }
    public int QuizzesTaken { get; set; }

    public virtual ApplicationUser User { get; set; } = null!;
}