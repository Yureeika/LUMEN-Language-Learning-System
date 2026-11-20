namespace LUMEN.Controllers;

using LUMEN.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Authorize(Roles = "SuperAdmin")]
public class SeedController(ApplicationDbContext context) : Controller
{
    public async Task<IActionResult> Index()
    {
        var quizCount = await context.Quizzes.CountAsync();

        TempData["SuccessMessage"] =
            quizCount == 0
                ? "No quizzes were seeded. Create quizzes manually from the admin panel."
                : $"There are currently {quizCount} quiz(es). No new quizzes or questions were created.";

        return RedirectToAction("Index", "Quizzes");
    }
}