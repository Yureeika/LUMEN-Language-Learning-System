using Microsoft.AspNetCore.Identity;
namespace LUMEN.Controllers;

using LUMEN.Data;
using LUMEN.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Authorize]
public class StudyGroupsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager) : Controller
{
    public async Task<IActionResult> Index()
    {
        var groups = await context.StudyGroups
            .Include(g => g.Owner)
            .Where(g => g.IsPublic)
            .ToListAsync();
        return View(groups);
    }

    public async Task<IActionResult> Details(int id)
    {
        var group = await context.StudyGroups
            .Include(g => g.Members)
            .ThenInclude(m => m.User)
            .FirstOrDefaultAsync(g => g.Id == id);

        if (group == null) return NotFound();
        return View(group);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Join(int id)
    {
        var user = await userManager.GetUserAsync(User);
        if (user == null) return Unauthorized();

        var exists = await context.GroupMembers.AnyAsync(m => m.StudyGroupId == id && m.UserId == user.Id);
        if (!exists)
        {
            context.GroupMembers.Add(new GroupMember { StudyGroupId = id, UserId = user.Id });
            await context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Details), new { id });
    }
}
