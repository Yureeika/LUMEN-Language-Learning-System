using Microsoft.AspNetCore.Identity;
namespace LUMEN.Controllers;

using LUMEN.Data;
using LUMEN.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Authorize]
public class NotesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager) : Controller
{
    public async Task<IActionResult> Index()
    {
        var user = await userManager.GetUserAsync(User);
        if (user == null) return RedirectToAction("Login", "Account");

        var notes = await context.Notes
            .Include(n => n.Lesson)
            .Where(n => n.UserId == user.Id)
            .OrderByDescending(n => n.UpdatedAt)
            .ToListAsync();

        return View(notes);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Save(int lessonId, string content)
    {
        var user = await userManager.GetUserAsync(User);
        if (user == null) return Unauthorized();

        var note = await context.Notes.FirstOrDefaultAsync(n => n.UserId == user.Id && n.LessonId == lessonId);
        if (note == null)
        {
            note = new Note { UserId = user.Id, LessonId = lessonId, Content = content };
            context.Notes.Add(note);
        }
        else
        {
            note.Content = content;
            note.UpdatedAt = DateTime.UtcNow;
            context.Notes.Update(note);
        }
        await context.SaveChangesAsync();
        return Json(new { success = true });
    }

    public async Task<IActionResult> Edit(int id)
    {
        var user = await userManager.GetUserAsync(User);
        if (user == null) return RedirectToAction("Login", "Account");

        var note = await context.Notes.FirstOrDefaultAsync(n => n.Id == id && n.UserId == user.Id);
        if (note == null) return NotFound();

        return View(note);
    }
}
