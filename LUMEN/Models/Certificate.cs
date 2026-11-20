namespace LUMEN.Models;

using System.ComponentModel.DataAnnotations;

public class Certificate
{
    [Key]
    public int Id { get; set; }

    [Required]
    public required string UserId { get; set; }

    public int SubjectId { get; set; }

    [Required]
    [StringLength(255)]
    public required string CertificateUrl { get; set; }

    [Required]
    [StringLength(100)]
    public required string CertificateCode { get; set; }

    public DateTime IssuedAt { get; set; } = DateTime.UtcNow;

    public virtual ApplicationUser User { get; set; } = null!;
    public virtual Subject Subject { get; set; } = null!;
}