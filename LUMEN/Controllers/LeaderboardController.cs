namespace LUMEN.Controllers;

using LUMEN.Data;
using LUMEN.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Authorize]
public class LeaderboardController(ApplicationDbContext context, UserManager<ApplicationUser> userManager) : Controller
{
    public async Task<IActionResult> Index()
    {
        var currentUser = await userManager.GetUserAsync(User);

        var users = await context.Users
            .Where(u => !u.IsSoftDeleted)
            .OrderByDescending(u => u.TotalXP)
            .Take(100)
            .Select(u => new
            {
                u.Id,
                u.FirstName,
                u.LastName,
                u.Nickname,
                u.TotalXP,
                u.Categories,
                QuizAttempts = context.QuizAttempts.Count(a => a.UserId == u.Id)
            })
            .ToListAsync();

        // Add rank manually
        var leaderboard = users.Select((u, index) => new
        {
            u.Id,
            u.FirstName,
            u.LastName,
            u.Nickname,
            u.TotalXP,
            u.Categories,
            u.QuizAttempts,
            Rank = index + 1
        }).ToList();

        ViewBag.Leaderboard = leaderboard;
        ViewBag.CurrentUserId = currentUser?.Id;
        ViewBag.CurrentUserRank = leaderboard.FirstOrDefault(u => u.Id == currentUser?.Id)?.Rank ?? 0;

        return View();
    }
}
