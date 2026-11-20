namespace LUMEN.Models;

using System.ComponentModel.DataAnnotations;

public class Resource
{
    [Key]
    public int Id { get; set; }

    public int LessonId { get; set; }

    [Required]
    [StringLength(150)]
    public required string Title { get; set; }

    [Required]
    [StringLength(255)]
    public required string FileUrl { get; set; }

    [StringLength(50)]
    public string? FileType { get; set; }

    public DateTime UploadedAt { get; set; } = DateTime.UtcNow;

    public virtual Lesson Lesson { get; set; } = null!;
}