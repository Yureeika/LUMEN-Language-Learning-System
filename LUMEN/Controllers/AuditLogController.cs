using Microsoft.AspNetCore.Identity;
using LUMEN.Models;
namespace LUMEN.Controllers;

using LUMEN.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Authorize(Roles = "SuperAdmin")]
public class AuditLogController(ApplicationDbContext context) : Controller
{
    public async Task<IActionResult> Index(string? userId, string? action)
    {
        var query = context.AuditLogs
            .Include(a => a.User)
            .OrderByDescending(a => a.CreatedAt)
            .AsQueryable();

        if (!string.IsNullOrEmpty(userId))
        {
            query = query.Where(a => a.UserId == userId);
        }

        if (!string.IsNullOrEmpty(action))
        {
            query = query.Where(a => a.Action.Contains(action));
        }

        var logs = await query.Take(100).ToListAsync();
        ViewBag.FilteredUserId = userId;
        ViewBag.FilteredAction = action;

        return View(logs);
    }
}
