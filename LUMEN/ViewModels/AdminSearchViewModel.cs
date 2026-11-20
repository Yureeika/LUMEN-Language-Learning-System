namespace LUMEN.ViewModels;

using LUMEN.Models;

public class AdminSearchViewModel
{
    public List<ApplicationUser> Users { get; set; } = new();
    public List<Subject> Subjects { get; set; } = new();
    public List<Announcement> Announcements { get; set; } = new();
}