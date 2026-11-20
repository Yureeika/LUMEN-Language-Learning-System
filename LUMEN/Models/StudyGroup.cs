namespace LUMEN.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class StudyGroup
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = "";

    public string? Description { get; set; }
    public string? Category { get; set; }
    public string? CreatedBy { get; set; }
    public bool IsPublic { get; set; } = true;  // ← ADD THIS
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [ForeignKey("CreatedBy")]
    public virtual ApplicationUser? Owner { get; set; }

    public virtual ICollection<GroupMember> Members { get; set; } = new List<GroupMember>();
}