namespace LUMEN.ViewModels;

using LUMEN.Models;

public class DashboardViewModel
{
    public ApplicationUser User { get; set; }
    public int EnrolledSubjectsCount { get; set; }
    public int CompletedLessonsCount { get; set; }
    public int TotalBadges { get; set; }
    public List<Enrollment> RecentEnrollments { get; set; } = [];
    public List<XPLog> RecentXPLogs { get; set; } = [];
    public List<Announcement> RecentAnnouncements { get; set; } = [];
}
