using Microsoft.AspNetCore.Identity;
namespace LUMEN.Controllers;

using LUMEN.Data;
using LUMEN.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Authorize(Roles = "SuperAdmin")]
public class ResourcesController(ApplicationDbContext context) : Controller
{
    public async Task<IActionResult> Index(int lessonId)
    {
        var resources = await context.Resources
            .Where(r => r.LessonId == lessonId)
            .ToListAsync();
        ViewBag.LessonId = lessonId;
        return View(resources);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("LessonId,Title,FileUrl,FileType")] Resource resource)
    {
        if (ModelState.IsValid)
        {
            resource.UploadedAt = DateTime.UtcNow;
            context.Resources.Add(resource);
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { lessonId = resource.LessonId });
        }
        ViewBag.LessonId = resource.LessonId;
        return View("Index", await context.Resources.Where(r => r.LessonId == resource.LessonId).ToListAsync());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id, int lessonId)
    {
        var resource = await context.Resources.FindAsync(id);
        if (resource != null)
        {
            context.Resources.Remove(resource);
            await context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index), new { lessonId });
    }
}
