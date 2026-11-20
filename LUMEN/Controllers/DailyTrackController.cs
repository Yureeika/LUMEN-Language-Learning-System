namespace LUMEN.Controllers;

using LUMEN.Data;
using LUMEN.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Authorize]
public class DailyTrackController(ApplicationDbContext context, UserManager<ApplicationUser> userManager) : Controller
{
    public async Task<IActionResult> Index()
    {
        var user = await userManager.GetUserAsync(User);
        if (user == null) return RedirectToAction("Login", "Account");

        var today = DateOnly.FromDateTime(DateTime.UtcNow);

        // Get or create today's track
        var track = await context.DailyTracks.FirstOrDefaultAsync(t => t.UserId == user.Id && t.TrackDate == today);
        if (track == null)
        {
            track = new DailyTrack { UserId = user.Id, TrackDate = today };
            context.DailyTracks.Add(track);
            await context.SaveChangesAsync();
        }

        // Get today's activities
        var todayQuizzes = await context.QuizAttempts
            .Include(q => q.Quiz)
            .ThenInclude(q => q.Lesson)
            .Where(q => q.UserId == user.Id && DateOnly.FromDateTime(q.CompletedAt ?? DateTime.UtcNow) == today)
            .ToListAsync();

        var todayBookmarks = await context.Bookmarks
            .Include(b => b.Lesson)
            .Where(b => b.UserId == user.Id && DateOnly.FromDateTime(b.CreatedAt) == today)
            .ToListAsync();

        var todayNotes = await context.Notes
            .Include(n => n.Lesson)
            .Where(n => n.UserId == user.Id && DateOnly.FromDateTime(n.CreatedAt) == today)
            .ToListAsync();

        var todayLabSessions = await context.VirtualLabSessions
            .Where(s => s.UserId == user.Id && DateOnly.FromDateTime(s.StartedAt) == today)
            .ToListAsync();

        // Get last 7 days for chart
        var last7Days = await context.DailyTracks
            .Where(t => t.UserId == user.Id && t.TrackDate >= today.AddDays(-7))
            .OrderBy(t => t.TrackDate)
            .ToListAsync();

        ViewBag.TodayQuizzes = todayQuizzes;
        ViewBag.TodayBookmarks = todayBookmarks;
        ViewBag.TodayNotes = todayNotes;
        ViewBag.TodayLabSessions = todayLabSessions;
        ViewBag.Last7Days = last7Days;

        return View(track);
    }
}