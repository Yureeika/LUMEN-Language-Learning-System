using LUMEN.Data;
using LUMEN.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace LUMEN.Controllers;

[Authorize]
public class LessonsController(ApplicationDbContext context) : Controller
{
    // =========================
    // LESSON LIST
    // =========================
    public async Task<IActionResult> Index(int? subjectId)
    {
        IQueryable<Lesson> query = context.Lessons
            .Include(l => l.Subject)
            .OrderBy(l => l.OrderIndex);

        // If a subject was selected, show only that subject's lessons.
        if (subjectId.HasValue)
        {
            query = query.Where(l => l.SubjectId == subjectId.Value);
        }

        // Admin can see both published and unpublished lessons.
        var lessons = await query.ToListAsync();

        ViewBag.SubjectId = subjectId;

        return View(lessons);
    }

    // =========================
    // LESSON DETAILS
    // =========================
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
            return NotFound();

        var lesson = await context.Lessons
            .Include(l => l.Subject)
            .Include(l => l.Resources)
            .FirstOrDefaultAsync(l => l.Id == id);

        if (lesson == null)
            return NotFound();

        return View(lesson);
    }

    // =========================
    // CREATE - GET
    // =========================
    [Authorize(Roles = "SuperAdmin")]
    public async Task<IActionResult> Create(int? subjectId)
    {
        var subjects = await context.Subjects
            .OrderBy(s => s.Title)
            .ToListAsync();

        ViewBag.SubjectId = new SelectList(
            subjects,
            "Id",
            "Title",
            subjectId
        );

        ViewBag.SelectedSubjectId = subjectId;

        return View(new Lesson
        {
            SubjectId = subjectId ?? 0,
            OrderIndex = 1,
            IsPublished = true
        });
    }

    // =========================
    // CREATE - POST
    // =========================
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<IActionResult> Create(
        [Bind("SubjectId,Title,Content,VideoUrl,OrderIndex,IsPublished")]
        Lesson lesson)
    {
        // Check subject
        if (lesson.SubjectId <= 0)
        {
            ModelState.AddModelError(
                "SubjectId",
                "Please select a subject."
            );
        }
        else
        {
            var subjectExists = await context.Subjects
                .AnyAsync(s => s.Id == lesson.SubjectId);

            if (!subjectExists)
            {
                ModelState.AddModelError(
                    "SubjectId",
                    "The selected subject does not exist."
                );
            }
        }

        // Check title
        if (string.IsNullOrWhiteSpace(lesson.Title))
        {
            ModelState.AddModelError(
                "Title",
                "Please enter a lesson title."
            );
        }

        // Check order
        if (lesson.OrderIndex < 1)
        {
            ModelState.AddModelError(
                "OrderIndex",
                "Lesson order must be 1 or greater."
            );
        }

        if (ModelState.IsValid)
        {
            lesson.CreatedAt = DateTime.UtcNow;

            context.Lessons.Add(lesson);

            await context.SaveChangesAsync();

            TempData["SuccessMessage"] =
                "Lesson created successfully.";

            return RedirectToAction(
                nameof(Index),
                new { subjectId = lesson.SubjectId }
            );
        }

        // Reload subjects if validation fails
        var subjects = await context.Subjects
            .OrderBy(s => s.Title)
            .ToListAsync();

        ViewBag.SubjectId = new SelectList(
            subjects,
            "Id",
            "Title",
            lesson.SubjectId
        );

        ViewBag.SelectedSubjectId = lesson.SubjectId;

        return View(lesson);
    }

    // =========================
    // EDIT - GET
    // =========================
    [Authorize(Roles = "SuperAdmin")]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
            return NotFound();

        var lesson = await context.Lessons
            .FindAsync(id);

        if (lesson == null)
            return NotFound();

        var subjects = await context.Subjects
            .OrderBy(s => s.Title)
            .ToListAsync();

        ViewBag.SubjectId = new SelectList(
            subjects,
            "Id",
            "Title",
            lesson.SubjectId
        );

        ViewBag.SelectedSubjectId = lesson.SubjectId;

        return View(lesson);
    }

    // =========================
    // EDIT - POST
    // =========================
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<IActionResult> Edit(
        int id,
        [Bind("Id,SubjectId,Title,Content,VideoUrl,OrderIndex,IsPublished")]
        Lesson lesson)
    {
        if (id != lesson.Id)
            return NotFound();

        // Check subject
        if (lesson.SubjectId <= 0)
        {
            ModelState.AddModelError(
                "SubjectId",
                "Please select a subject."
            );
        }
        else
        {
            var subjectExists = await context.Subjects
                .AnyAsync(s => s.Id == lesson.SubjectId);

            if (!subjectExists)
            {
                ModelState.AddModelError(
                    "SubjectId",
                    "The selected subject does not exist."
                );
            }
        }

        // Check title
        if (string.IsNullOrWhiteSpace(lesson.Title))
        {
            ModelState.AddModelError(
                "Title",
                "Please enter a lesson title."
            );
        }

        // Check order
        if (lesson.OrderIndex < 1)
        {
            ModelState.AddModelError(
                "OrderIndex",
                "Lesson order must be 1 or greater."
            );
        }

        if (ModelState.IsValid)
        {
            try
            {
                context.Update(lesson);

                await context.SaveChangesAsync();

                TempData["SuccessMessage"] =
                    "Lesson updated successfully.";

                return RedirectToAction(
                    nameof(Index),
                    new { subjectId = lesson.SubjectId }
                );
            }
            catch (DbUpdateConcurrencyException)
            {
                var exists = await context.Lessons
                    .AnyAsync(l => l.Id == lesson.Id);

                if (!exists)
                    return NotFound();

                throw;
            }
        }

        // Reload subjects if validation fails
        var subjects = await context.Subjects
            .OrderBy(s => s.Title)
            .ToListAsync();

        ViewBag.SubjectId = new SelectList(
            subjects,
            "Id",
            "Title",
            lesson.SubjectId
        );

        ViewBag.SelectedSubjectId = lesson.SubjectId;

        return View(lesson);
    }
}