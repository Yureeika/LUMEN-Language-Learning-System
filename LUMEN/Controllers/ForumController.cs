using Microsoft.AspNetCore.Identity;
namespace LUMEN.Controllers;

using LUMEN.Data;
using LUMEN.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Authorize]
public class ForumController(ApplicationDbContext context, UserManager<ApplicationUser> userManager) : Controller
{
    public async Task<IActionResult> Index()
    {
        var threads = await context.DiscussionThreads
            .Include(t => t.User)
            .Include(t => t.Posts)
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync();
        return View(threads);
    }

    public IActionResult Create() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(DiscussionThread thread)
    {
        var user = await userManager.GetUserAsync(User);
        if (user == null) return Unauthorized();

        thread.UserId = user.Id;
        thread.CreatedAt = DateTime.UtcNow;
        context.DiscussionThreads.Add(thread);
        await context.SaveChangesAsync();
        return RedirectToAction(nameof(Thread), new { id = thread.Id });
    }

    public async Task<IActionResult> Thread(int id)
    {
        var thread = await context.DiscussionThreads
            .Include(t => t.User)
            .Include(t => t.Posts)
            .ThenInclude(p => p.User)
            .FirstOrDefaultAsync(t => t.Id == id);

        if (thread == null) return NotFound();
        return View(thread);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddPost(int threadId, string content)
    {
        var user = await userManager.GetUserAsync(User);
        if (user == null) return Unauthorized();

        var post = new Post
        {
            ThreadId = threadId,
            UserId = user.Id,
            Content = content,
            CreatedAt = DateTime.UtcNow
        };
        context.Posts.Add(post);
        await context.SaveChangesAsync();
        return RedirectToAction(nameof(Thread), new { id = threadId });
    }
}

