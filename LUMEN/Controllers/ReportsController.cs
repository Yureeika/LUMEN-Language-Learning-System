using Microsoft.AspNetCore.Identity;
namespace LUMEN.Controllers;

using LUMEN.Data;
using LUMEN.Models;
using LUMEN.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Authorize]
public class ReportsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager) : Controller
{
    [Authorize(Roles = "SuperAdmin")]
    public async Task<IActionResult> AdminReport(string? userId)
    {
        if (string.IsNullOrEmpty(userId))
        {
            var users = await context.Users.ToListAsync();
            ViewBag.Users = users;
            return View(new ReportViewModel { UserId = "", UserName = "Select a user" });
        }

        var user = await context.Users.FindAsync(userId);
        if (user == null) return NotFound();

        var enrolled = await context.Enrollments.CountAsync(e => e.UserId == userId);
        var completed = await context.Enrollments.CountAsync(e => e.UserId == userId && e.IsCompleted);
        var passed = await context.QuizAttempts.CountAsync(q => q.UserId == userId && q.Passed);
        var failed = await context.QuizAttempts.CountAsync(q => q.UserId == userId && !q.Passed);

        var model = new ReportViewModel
        {
            UserId = user.Id,
            UserName = $"{user.FirstName} {user.LastName}",
            TotalXP = user.TotalXP,
            CurrentLevel = int.TryParse(user.CurrentLevel, out var lvl) ? lvl : 0,
            SubjectsEnrolled = enrolled,
            SubjectsCompleted = completed,
            QuizzesPassed = passed,
            QuizzesFailed = failed,
            MonthlyXP = []
        };

        return View(model);
    }

    public async Task<IActionResult> StudentReport()
    {
        var user = await userManager.GetUserAsync(User);
        if (user == null) return RedirectToAction("Login", "Account");

        var enrolled = await context.Enrollments.CountAsync(e => e.UserId == user.Id);
        var passed = await context.QuizAttempts.CountAsync(q => q.UserId == user.Id && q.Passed);
        var failed = await context.QuizAttempts.CountAsync(q => q.UserId == user.Id && !q.Passed);

        var model = new ReportViewModel
        {
            UserId = user.Id,
            UserName = $"{user.FirstName} {user.LastName}",
            TotalXP = user.TotalXP,
            CurrentLevel = int.TryParse(user.CurrentLevel, out var lvl) ? lvl : 0,
            SubjectsEnrolled = enrolled,
            SubjectsCompleted = 0,
            QuizzesPassed = passed,
            QuizzesFailed = failed,
            MonthlyXP = []
        };

        return View(model);
    }
}


