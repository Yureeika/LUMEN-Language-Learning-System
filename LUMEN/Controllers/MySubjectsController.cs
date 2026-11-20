namespace LUMEN.Controllers;

using LUMEN.Data;
using LUMEN.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Authorize]
public class MySubjectsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager) : Controller
{
    public async Task<IActionResult> Index()
    {
        var user = await userManager.GetUserAsync(User);
        if (user == null) return RedirectToAction("Login", "Account");

        var enrollments = await context.Enrollments
            .Include(e => e.Subject)
            .Where(e => e.UserId == user.Id)
            .ToListAsync();

        return View(enrollments);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Enroll(int subjectId)
    {
        var user = await userManager.GetUserAsync(User);
        if (user == null) return Unauthorized();

        var exists = await context.Enrollments.AnyAsync(e => e.UserId == user.Id && e.SubjectId == subjectId);
        if (!exists)
        {
            context.Enrollments.Add(new Enrollment { UserId = user.Id, SubjectId = subjectId });
            await context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }
}