namespace LUMEN.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class XPLog
{
    public int Id { get; set; }

    [Required]
    public string UserId { get; set; } = "";

    [ForeignKey("UserId")]
    public virtual ApplicationUser User { get; set; } = null!;

    public int Amount { get; set; }
    public string Reason { get; set; } = "";
    public DateTime Date { get; set; } = DateTime.UtcNow;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}