using Microsoft.AspNetCore.Identity;
namespace LUMEN.Controllers;

using LUMEN.Data;
using LUMEN.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Authorize]
public class BookmarksController(ApplicationDbContext context, UserManager<ApplicationUser> userManager) : Controller
{
    public async Task<IActionResult> Index()
    {
        var user = await userManager.GetUserAsync(User);
        if (user == null) return RedirectToAction("Login", "Account");

        var bookmarks = await context.Bookmarks
            .Include(b => b.Lesson).ThenInclude(l => l.Subject)
            .Where(b => b.UserId == user.Id)
            .OrderByDescending(b => b.CreatedAt)
            .ToListAsync();

        return View(bookmarks);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Add(int lessonId)
    {
        var user = await userManager.GetUserAsync(User);
        if (user == null) return Unauthorized();

        var exists = await context.Bookmarks.AnyAsync(b => b.UserId == user.Id && b.LessonId == lessonId);
        if (!exists)
        {
            context.Bookmarks.Add(new Bookmark { UserId = user.Id, LessonId = lessonId });
            await context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Remove(int id)
    {
        var user = await userManager.GetUserAsync(User);
        if (user == null) return Unauthorized();

        var bookmark = await context.Bookmarks.FirstOrDefaultAsync(b => b.Id == id && b.UserId == user.Id);
        if (bookmark != null)
        {
            context.Bookmarks.Remove(bookmark);
            await context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }
}
