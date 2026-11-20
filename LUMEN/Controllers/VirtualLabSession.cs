namespace LUMEN.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class VirtualLabSession
{
    public int Id { get; set; }

    [Required]
    public string UserId { get; set; } = "";

    [ForeignKey("UserId")]
    public virtual ApplicationUser User { get; set; } = null!;

    [Required]
    public string LabType { get; set; } = "";

    public string? Code { get; set; }  // ← ADD THIS LINE

    public int XPEarned { get; set; }
    public DateTime StartedAt { get; set; } = DateTime.UtcNow;
    public DateTime? CompletedAt { get; set; }
}