namespace LUMEN.Controllers;

using LUMEN.Data;
using LUMEN.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

[Authorize(Roles = "SuperAdmin")]
public class QuizzesController(ApplicationDbContext context) : Controller
{
    // =========================
    // QUIZ LIST
    // =========================
    public async Task<IActionResult> Index()
    {
        var quizzes = await context.Quizzes
            .Include(q => q.Lesson)
            .ThenInclude(l => l.Subject)
            .Include(q => q.Questions)
            .OrderBy(q => q.Title)
            .ToListAsync();

        return View(quizzes);
    }

    // =========================
    // CREATE QUIZ - GET
    // =========================
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var lessons = await context.Lessons
            .Include(l => l.Subject)
            .OrderBy(l => l.Subject!.Title)
            .ThenBy(l => l.OrderIndex)
            .ToListAsync();

        ViewBag.Lessons = new SelectList(
            lessons,
            "Id",
            "Title"
        );

        return View(new Quiz
        {
            Title = "",
            Description = "",
            TimeLimitMinutes = 10,
            PassingScore = 70,
            IsPublished = true
        });
    }
    // =========================
    // CREATE QUIZ - POST
    // =========================
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Quiz quiz)
    {
        // Validate lesson
        if (quiz.LessonId <= 0)
        {
            ModelState.AddModelError(
                nameof(quiz.LessonId),
                "Please select a lesson."
            );
        }
        else
        {
            var lessonExists = await context.Lessons
                .AnyAsync(l => l.Id == quiz.LessonId);

            if (!lessonExists)
            {
                ModelState.AddModelError(
                    nameof(quiz.LessonId),
                    "The selected lesson does not exist."
                );
            }
        }

        // Validate title
        if (string.IsNullOrWhiteSpace(quiz.Title))
        {
            ModelState.AddModelError(
                nameof(quiz.Title),
                "Please enter a quiz title."
            );
        }

        // Validate time limit
        if (quiz.TimeLimitMinutes < 1)
        {
            ModelState.AddModelError(
                nameof(quiz.TimeLimitMinutes),
                "Time limit must be at least 1 minute."
            );
        }

        // Validate passing score
        if (quiz.PassingScore < 0 || quiz.PassingScore > 100)
        {
            ModelState.AddModelError(
                nameof(quiz.PassingScore),
                "Passing score must be between 0 and 100."
            );
        }

        // If validation failed, return to form
        if (!ModelState.IsValid)
        {
            await LoadLessons(quiz.LessonId);
            return View(quiz);
        }

        try
        {
            quiz.CreatedAt = DateTime.UtcNow;

            context.Quizzes.Add(quiz);

            await context.SaveChangesAsync();

            TempData["SuccessMessage"] =
                "Quiz created successfully. Now add questions to it.";

            return RedirectToAction(
                "Index",
                "Questions",
                new { quizId = quiz.Id }
            );
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(
                "",
                "The quiz could not be created. " + ex.Message
            );

            await LoadLessons(quiz.LessonId);

            return View(quiz);
        }
    }
    private async Task LoadLessons(int? selectedLessonId = null)
    {
        var lessons = await context.Lessons
            .Include(l => l.Subject)
            .OrderBy(l => l.Subject!.Title)
            .ThenBy(l => l.OrderIndex)
            .ToListAsync();

        ViewBag.Lessons = new SelectList(
            lessons,
            "Id",
            "Title",
            selectedLessonId
        );
    }

    // =========================
    // DELETE QUIZ
    // =========================
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var quiz = await context.Quizzes
            .Include(q => q.Questions)
            .FirstOrDefaultAsync(q => q.Id == id);

        if (quiz == null)
            return NotFound();

        context.Quizzes.Remove(quiz);

        await context.SaveChangesAsync();

        TempData["SuccessMessage"] =
            "Quiz and its questions were deleted successfully.";

        return RedirectToAction(nameof(Index));
    }
}