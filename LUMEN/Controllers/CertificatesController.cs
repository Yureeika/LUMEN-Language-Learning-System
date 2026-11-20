using Microsoft.AspNetCore.Identity;
using LUMEN.Models;
namespace LUMEN.Controllers;

using LUMEN.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Authorize]
public class CertificatesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager) : Controller
{
    public async Task<IActionResult> Index()
    {
        var user = await userManager.GetUserAsync(User);
        if (user == null) return RedirectToAction("Login", "Account");

        var certificates = await context.Certificates
            .Include(c => c.Subject)
            .Where(c => c.UserId == user.Id)
            .OrderByDescending(c => c.IssuedAt)
            .ToListAsync();

        return View(certificates);
    }
}
