namespace LUMEN.Models;

using System.ComponentModel.DataAnnotations;

public class UserSetting
{
    [Key]
    public int Id { get; set; }

    [Required]
    public required string UserId { get; set; }

    public string Theme { get; set; } = "Light";
    public bool EmailNotifications { get; set; } = true;
    public bool PushNotifications { get; set; } = false;
    public string Language { get; set; } = "en-US";

    public virtual ApplicationUser User { get; set; } = null!;
}