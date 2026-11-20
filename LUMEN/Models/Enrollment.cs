namespace LUMEN.Models;

using System.ComponentModel.DataAnnotations;

public class Enrollment
{
    [Key]
    public int Id { get; set; }

    [Required]
    public required string UserId { get; set; }

    public int SubjectId { get; set; }

    public DateTime EnrolledAt { get; set; } = DateTime.UtcNow;
    public bool IsCompleted { get; set; } = false;
    public DateTime? CompletedAt { get; set; }

    public virtual ApplicationUser User { get; set; } = null!;
    public virtual Subject Subject { get; set; } = null!;
}