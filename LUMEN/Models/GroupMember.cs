namespace LUMEN.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class GroupMember
{
    public int Id { get; set; }

    [Required]
    public string UserId { get; set; } = "";

    [ForeignKey("UserId")]
    public virtual ApplicationUser User { get; set; } = null!;

    public int StudyGroupId { get; set; }

    [ForeignKey("StudyGroupId")]
    public virtual StudyGroup StudyGroup { get; set; } = null!;

    public string Role { get; set; } = "Member";
    public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
}