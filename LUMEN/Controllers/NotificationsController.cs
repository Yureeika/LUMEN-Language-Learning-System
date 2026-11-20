namespace LUMEN.Controllers;

using LUMEN.Data;
using LUMEN.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Authorize]
public class NotificationsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager) : Controller
{
    public async Task<IActionResult> Index()
    {
        var user = await userManager.GetUserAsync(User);
        if (user == null) return RedirectToAction("Login", "Account");

        var notifications = await context.Notifications
            .Where(n => n.UserId == user.Id)
            .OrderByDescending(n => n.CreatedAt)
            .ToListAsync();

        return View(notifications);
    }

    [HttpPost]
    public async Task<IActionResult> MarkAsRead(int id)
    {
        var user = await userManager.GetUserAsync(User);
        if (user == null) return Unauthorized();

        var notification = await context.Notifications.FirstOrDefaultAsync(n => n.Id == id && n.UserId == user.Id);
        if (notification != null)
        {
            notification.IsRead = true;
            await context.SaveChangesAsync();
        }
        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> UnreadCount()
    {
        var user = await userManager.GetUserAsync(User);
        if (user == null) return Json(0);

        var count = await context.Notifications.CountAsync(n => n.UserId == user.Id && !n.IsRead);
        return Json(count);
    }
}