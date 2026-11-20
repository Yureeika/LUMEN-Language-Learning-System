namespace LUMEN.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Feedback
{
    public int Id { get; set; }

    [Required]
    public string UserId { get; set; } = "";

    [ForeignKey("UserId")]
    public ApplicationUser User { get; set; } = null!;

    [Required]
    public string Type { get; set; } = "Feedback"; // Feedback, Complaint, Review

    [Required]
    public string Subject { get; set; } = "";

    [Required]
    public string Message { get; set; } = "";

    public bool IsRead { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}