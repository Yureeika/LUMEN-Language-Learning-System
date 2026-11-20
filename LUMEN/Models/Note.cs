namespace LUMEN.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Note
{
    public int Id { get; set; }
    
    [Required]
    public string UserId { get; set; } = "";
    
    [ForeignKey("UserId")]
    public virtual ApplicationUser User { get; set; } = null!;
    
    public int? SubjectId { get; set; }
    
    [ForeignKey("SubjectId")]
    public virtual Subject? Subject { get; set; }
    
    public int? LessonId { get; set; }
    
    [ForeignKey("LessonId")]
    public virtual Lesson? Lesson { get; set; }
    
    public string Title { get; set; } = "";
    public string Content { get; set; } = "";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}
