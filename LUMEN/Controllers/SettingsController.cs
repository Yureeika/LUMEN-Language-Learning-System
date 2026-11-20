namespace LUMEN.Controllers;

using LUMEN.Data;
using LUMEN.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Authorize]
public class SettingsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager) : Controller
{
    public async Task<IActionResult> Index()
    {
        var user = await userManager.GetUserAsync(User);
        if (user == null) return RedirectToAction("Login", "Account");

        var settings = await context.UserSettings.FirstOrDefaultAsync(s => s.UserId == user.Id);
        if (settings == null)
        {
            settings = new UserSetting { UserId = user.Id };
            context.UserSettings.Add(settings);
            await context.SaveChangesAsync();
        }

        return View(settings);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateSettings(UserSetting settings)
    {
        var user = await userManager.GetUserAsync(User);
        if (user == null) return Unauthorized();

        var existing = await context.UserSettings.FirstOrDefaultAsync(s => s.UserId == user.Id);
        if (existing == null)
        {
            settings.UserId = user.Id;
            context.UserSettings.Add(settings);
        }
        else
        {
            existing.Theme = settings.Theme;
            existing.EmailNotifications = settings.EmailNotifications;
            existing.PushNotifications = settings.PushNotifications;
            existing.Language = settings.Language;
            context.UserSettings.Update(existing);
        }
        await context.SaveChangesAsync();
        TempData["SuccessMessage"] = "Settings updated successfully.";
        return RedirectToAction(nameof(Index));
    }

    public IActionResult ChangePassword() => View();
}