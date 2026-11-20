using Microsoft.AspNetCore.Identity;
using LUMEN.Models;
namespace LUMEN.Controllers;

using LUMEN.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Authorize]
public class BadgesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager) : Controller
{
    public async Task<IActionResult> Index()
    {
        var user = await userManager.GetUserAsync(User);
        if (user == null) return RedirectToAction("Login", "Account");

        var allBadges = await context.Badges.Where(b => b.IsActive).ToListAsync();
        var userBadges = await context.UserBadges
            .Include(ub => ub.Badge)
            .Where(ub => ub.UserId == user.Id)
            .ToListAsync();

        ViewBag.UserBadges = userBadges;
        return View(allBadges);
    }
}
