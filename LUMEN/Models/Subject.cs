namespace LUMEN.Models;

using System.ComponentModel.DataAnnotations;

public class Subject
{
    public int Id { get; set; }

    [Required]
    public string Title { get; set; } = "";

    public string? Description { get; set; }

    [Required]
    public string Category { get; set; } = "";

    public string? ImageUrl { get; set; }

    public bool IsActive { get; set; } = true;

    public string Level { get; set; } = "Basic";

    public decimal Price { get; set; } = 0;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation property
    public virtual ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();
}