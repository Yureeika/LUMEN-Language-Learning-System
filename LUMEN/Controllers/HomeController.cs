namespace LUMEN.Controllers;

using LUMEN.Data;
using LUMEN.Models;
using LUMEN.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class HomeController(
    ApplicationDbContext context,
    UserManager<ApplicationUser> userManager,
    SignInManager<ApplicationUser> signInManager,
    ILogger<HomeController> logger) : Controller
{
    // Public landing page
    public IActionResult Index()
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            if (User.IsInRole("SuperAdmin"))
            {
                return RedirectToAction("AdminDashboard", "Home");
            }
            return RedirectToAction("Dashboard", "Home");
        }
        return View("Landing");
    }

  

    // Admin Dashboard with Analytics
    [Authorize(Roles = "SuperAdmin")]
    public async Task<IActionResult> AdminDashboard()
    {
        var totalUsers = await context.Users.CountAsync();
        var totalSubjects = await context.Subjects.CountAsync();
        var totalLessons = await context.Lessons.CountAsync();
        var totalQuizzes = await context.Quizzes.CountAsync();
        var totalEnrollments = await context.Enrollments.CountAsync();
        var totalQuizAttempts = await context.QuizAttempts.CountAsync();
        var totalAnnouncements = await context.Announcements.CountAsync();

        var usersByCategory = await context.Users
            .GroupBy(u => u.Categories)
            .Select(g => new { Category = g.Key ?? "Not Set", Count = g.Count() })
            .ToListAsync();

        var sevenDaysAgo = DateTime.UtcNow.AddDays(-7);
        var recentRegistrationsRaw = await context.Users
    .Where(u => u.CreatedAt >= sevenDaysAgo)
    .GroupBy(u => u.CreatedAt.Date)
    .Select(g => new { Date = g.Key, Count = g.Count() })
    .OrderBy(x => x.Date)
    .ToListAsync();

        var recentRegistrations = recentRegistrationsRaw
            .Select(g => new { Date = g.Date.ToString("yyyy-MM-dd"), g.Count })
            .ToList();

        var recentQuizAttemptsRaw = await context.QuizAttempts
    .Where(q => q.CompletedAt >= sevenDaysAgo)
    .GroupBy(q => q.CompletedAt!.Value.Date)
    .Select(g => new { Date = g.Key, Count = g.Count() })
    .OrderBy(x => x.Date)
    .ToListAsync();

        var recentQuizAttempts = recentQuizAttemptsRaw
            .Select(g => new { Date = g.Date.ToString("yyyy-MM-dd"), g.Count })
            .ToList();

        var topSubjects = await context.Enrollments
            .GroupBy(e => e.SubjectId)
            .Select(g => new { SubjectId = g.Key, Count = g.Count() })
            .OrderByDescending(x => x.Count)
            .Take(5)
            .ToListAsync();

        var topSubjectDetails = new List<object>();
        foreach (var item in topSubjects)
        {
            var subject = await context.Subjects.FindAsync(item.SubjectId);
            if (subject != null)
            {
                topSubjectDetails.Add(new { subject.Title, item.Count });
            }
        }

        var fiveMinutesAgo = DateTime.UtcNow.AddMinutes(-5);
        var onlineUsers = await context.Users
            .CountAsync(u => u.LastLoginAt >= fiveMinutesAgo);

        var recentUsers = await context.Users
            .OrderByDescending(u => u.CreatedAt)
            .Take(5)
            .Select(u => new { u.FirstName, u.LastName, u.Email, u.CreatedAt, u.Categories })
            .ToListAsync();

        var recentAttempts = await context.QuizAttempts
            .Include(a => a.User)
            .Include(a => a.Quiz)
            .OrderByDescending(a => a.CompletedAt)
            .Take(5)
            .Select(a => new
            {
                UserName = a.User.FirstName + " " + a.User.LastName,
                QuizTitle = a.Quiz != null ? a.Quiz.Title : "Unknown",
                a.Score,
                a.CompletedAt
            })
            .ToListAsync();

        var passedAttempts = await context.QuizAttempts.CountAsync(a => a.Passed);
        var passRate = totalQuizAttempts > 0 ? (passedAttempts * 100 / totalQuizAttempts) : 0;

        double avgScore = 0;
        if (totalQuizAttempts > 0)
        {
            var allAttempts = await context.QuizAttempts
                .Select(a => new { a.Score, a.TotalPoints })
                .ToListAsync();

            avgScore = allAttempts
                .Where(a => a.TotalPoints > 0)
                .Average(a => (double)a.Score / a.TotalPoints * 100);
        }

        ViewBag.TotalUsers = totalUsers;
        ViewBag.TotalSubjects = totalSubjects;
        ViewBag.TotalLessons = totalLessons;
        ViewBag.TotalQuizzes = totalQuizzes;
        ViewBag.TotalEnrollments = totalEnrollments;
        ViewBag.TotalQuizAttempts = totalQuizAttempts;
        ViewBag.TotalAnnouncements = totalAnnouncements;
        ViewBag.OnlineUsers = onlineUsers;
        ViewBag.PassRate = passRate;
        ViewBag.AvgScore = Math.Round(avgScore, 1);

        ViewBag.UsersByCategory = usersByCategory;
        ViewBag.RecentRegistrations = recentRegistrations;
        ViewBag.RecentQuizAttempts = recentQuizAttempts;
        ViewBag.TopSubjects = topSubjectDetails;
        ViewBag.RecentUsers = recentUsers;
        ViewBag.RecentAttempts = recentAttempts;

        return View();
    }

    // Profile Page
    [Authorize]
    public async Task<IActionResult> Profile()
    {
        var user = await userManager.GetUserAsync(User);
        if (user == null) return RedirectToAction("Login", "Account");

        ViewBag.User = user;
        ViewBag.IsAdmin = User.IsInRole("SuperAdmin");

        var enrollments = await context.Enrollments
            .Where(e => e.UserId == user.Id)
            .Include(e => e.Subject)
            .ToListAsync();
        ViewBag.Enrollments = enrollments;

        var attempts = await context.QuizAttempts
            .Where(a => a.UserId == user.Id)
            .Include(a => a.Quiz)
            .OrderByDescending(a => a.CompletedAt)
            .Take(10)
            .ToListAsync();
        ViewBag.Attempts = attempts;

        return View();
    }

    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateProfile(string Nickname, string FirstName, string LastName)
    {
        var user = await userManager.GetUserAsync(User);
        if (user == null) return RedirectToAction("Login", "Account");

        user.Nickname = Nickname;
        user.FirstName = FirstName;
        user.LastName = LastName;

        await context.SaveChangesAsync();

        TempData["Success"] = "Profile updated successfully!";
        return RedirectToAction("Profile");
    }

    // Admin Search
    [Authorize(Roles = "SuperAdmin")]
    public async Task<IActionResult> AdminSearch(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
            return RedirectToAction("AdminDashboard");

        var results = new AdminSearchViewModel();

        results.Users = await context.Users
            .Where(u => u.FirstName.Contains(query) ||
                        u.LastName.Contains(query) ||
                        u.Email.Contains(query))
            .Take(20)
            .ToListAsync();

        results.Subjects = await context.Subjects
            .Where(s => s.Title.Contains(query) ||
                        (s.Description != null && s.Description.Contains(query)))
            .Take(20)
            .ToListAsync();

        results.Announcements = await context.Announcements
            .Where(a => a.Title.Contains(query) ||
                        (a.Content != null && a.Content.Contains(query)))
            .Take(10)
            .ToListAsync();

        ViewBag.Query = query;
        ViewBag.Results = results;
        return View();
    }

    // Student Dashboard
    [Authorize]
    public async Task<IActionResult> Dashboard()
    {
        var user = await userManager.GetUserAsync(User);
        if (user == null) return RedirectToAction("Login", "Account");

        ViewBag.User = user;

        // Student's enrollments
        var enrollments = await context.Enrollments
            .Where(e => e.UserId == user.Id)
            .Include(e => e.Subject)
            .ToListAsync();
        ViewBag.Enrollments = enrollments;

        // Student's quiz attempts
        var attempts = await context.QuizAttempts
            .Where(a => a.UserId == user.Id)
            .Include(a => a.Quiz)
            .OrderByDescending(a => a.CompletedAt)
            .ToListAsync();
        ViewBag.Attempts = attempts;

        // Student's badges
        var badges = await context.UserBadges
            .Where(ub => ub.UserId == user.Id)
            .Include(ub => ub.Badge)
            .ToListAsync();
        ViewBag.Badges = badges;

        // KPI Counts
        var totalXP = user.TotalXP;
        var enrolledCount = enrollments.Count;
        var badgesCount = badges.Count;
        var streak = 1; // TODO: Calculate from DailyTrack
        var quizAttemptsCount = attempts.Count;
        var passedQuizzes = attempts.Count(a => a.Passed);

        // XP Over Time (Last 7 days)
        var sevenDaysAgo = DateTime.UtcNow.AddDays(-7);
        var xpOverTime = await context.XPLogs
    .Where(x => x.UserId == user.Id && x.CreatedAt >= sevenDaysAgo)
    .GroupBy(x => x.CreatedAt.Date)
    .Select(g => new { Date = g.Key, XP = g.Sum(x => x.Amount) })
    .OrderBy(x => x.Date)
    .ToListAsync();

        // If no XP logs, use quiz attempts as fallback
        if (!xpOverTime.Any())
        {
            var xpFromAttempts = attempts
                .Where(a => a.CompletedAt >= sevenDaysAgo)
                .GroupBy(a => a.CompletedAt!.Value.Date)
                .Select(g => new { Date = g.Key, XP = g.Sum(a => a.Score) })
                .OrderBy(x => x.Date)
                .ToList();
            xpOverTime = xpFromAttempts.Select(x => new { Date = x.Date, XP = x.XP }).ToList();
        }

        // Quiz Performance (Last 7 days)
        var quizPerformance = attempts
            .Where(a => a.CompletedAt >= sevenDaysAgo)
            .GroupBy(a => a.CompletedAt!.Value.Date)
            .Select(g => new { Date = g.Key, Count = g.Count(), AvgScore = g.Average(a => (double)a.Score / Math.Max(1, a.TotalPoints) * 100) })
            .OrderBy(x => x.Date)
            .ToList();

        // Subject Progress
        var subjectProgress = new List<object>();
        foreach (var enrollment in enrollments.Take(5))
        {
            var lessons = await context.Lessons
                .Where(l => l.SubjectId == enrollment.SubjectId)
                .CountAsync();

            var completedLessons = await context.Notes
    .Where(n => n.UserId == user.Id && n.SubjectId == enrollment.SubjectId)
    .CountAsync();

            var progress = lessons > 0 ? (completedLessons * 100 / lessons) : 0;
            subjectProgress.Add(new { Title = enrollment.Subject?.Title ?? "Unknown", Progress = Math.Min(progress, 100), Lessons = lessons });
        }
        ViewBag.SubjectProgress = subjectProgress;

        // Recent Activity
        var recentActivity = new List<object>();
        foreach (var attempt in attempts.Take(5))
        {
            recentActivity.Add(new
            {
                Type = "Quiz",
                Title = attempt.Quiz?.Title ?? "Unknown Quiz",
                Description = $"Scored {attempt.Score}/{attempt.TotalPoints}",
                Date = attempt.CompletedAt ?? DateTime.UtcNow,
                Passed = attempt.Passed
            });
        }
        ViewBag.RecentActivity = recentActivity;

        // Pass rate
        var passRate = quizAttemptsCount > 0 ? (passedQuizzes * 100 / quizAttemptsCount) : 0;
        var avgScore = quizAttemptsCount > 0
            ? attempts.Average(a => (double)a.Score / Math.Max(1, a.TotalPoints) * 100)
            : 0;

        ViewBag.TotalXP = totalXP;
        ViewBag.EnrolledCount = enrolledCount;
        ViewBag.BadgesCount = badgesCount;
        ViewBag.Streak = streak;
        ViewBag.QuizAttemptsCount = quizAttemptsCount;
        ViewBag.PassedQuizzes = passedQuizzes;
        ViewBag.PassRate = passRate;
        ViewBag.AvgScore = Math.Round(avgScore, 1);
        ViewBag.XPOverTime = xpOverTime;
        ViewBag.QuizPerformance = quizPerformance;

        return View();
    }

    // Get Notifications (Feedback/Complaints)
    [Authorize(Roles = "SuperAdmin")]
    public async Task<IActionResult> GetNotifications()
    {
        var feedbacks = await context.Feedbacks
            .Include(f => f.User)
            .OrderByDescending(f => f.CreatedAt)
            .Take(10)
            .Select(f => new
            {
                f.Id,
                f.Subject,
                f.Message,
                f.CreatedAt,
                UserName = f.User.FirstName + " " + f.User.LastName,
                f.Type
            })
            .ToListAsync();

        return Json(new { count = feedbacks.Count, items = feedbacks });
    }

    // Track user activity
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> TrackActivity()
    {
        var user = await userManager.GetUserAsync(User);
        if (user != null)
        {
            user.LastLoginAt = DateTime.UtcNow;
            await context.SaveChangesAsync();
        }
        return Ok();
    }

    public IActionResult About() => View();
    public IActionResult Contact() => View();
}
