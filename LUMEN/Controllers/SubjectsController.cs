namespace LUMEN.Controllers;

using LUMEN.Data;
using LUMEN.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

[Authorize]
public class SubjectsController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public SubjectsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index(string category, string level, string search)
    {
        var user = await _userManager.GetUserAsync(User);
        var isAdmin = User.IsInRole("SuperAdmin");

        var query = _context.Subjects.Where(s => s.IsActive);

        // Students only see their categories
        if (!isAdmin && user != null && !string.IsNullOrEmpty(user.Categories))
        {
            var userCats = user.Categories.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                           .Select(c => c.Trim())
                                           .ToList();
            query = query.Where(s => userCats.Contains(s.Category));
        }

        if (!string.IsNullOrEmpty(category))
            query = query.Where(s => s.Category == category);

        if (!string.IsNullOrEmpty(level))
            query = query.Where(s => s.Level == level);

        if (!string.IsNullOrEmpty(search))
            query = query.Where(s => s.Title.Contains(search) || (s.Description != null && s.Description.Contains(search)));

        var subjects = await query
            .OrderBy(s => s.Level == "Basic" ? 0 : s.Level == "Intermediate" ? 1 : 2)
            .ThenBy(s => s.Title)
            .ToListAsync();

        // Get enrollment counts for each subject
        var enrollmentCounts = await _context.Enrollments
            .GroupBy(e => e.SubjectId)
            .Select(g => new { SubjectId = g.Key, Count = g.Count() })
            .ToDictionaryAsync(x => x.SubjectId, x => x.Count);

        // Check which subjects the current user is enrolled in
        var userEnrollments = new HashSet<int>();
        if (user != null)
        {
            userEnrollments = await _context.Enrollments
                .Where(e => e.UserId == user.Id)
                .Select(e => e.SubjectId)
                .ToHashSetAsync();
        }

        ViewBag.Categories = await _context.Subjects
            .Where(s => s.IsActive)
            .Select(s => s.Category)
            .Distinct()
            .ToListAsync();
        ViewBag.CurrentCategory = category;
        ViewBag.CurrentLevel = level;
        ViewBag.Search = search;
        ViewBag.IsAdmin = isAdmin;
        ViewBag.UserCategories = user?.Categories ?? "";
        ViewBag.EnrollmentCounts = enrollmentCounts;
        ViewBag.UserEnrollments = userEnrollments;

        return View(subjects);
    }

    public async Task<IActionResult> Details(int id)
    {
        var subject = await _context.Subjects.FindAsync(id);
        if (subject == null) return NotFound();

        var user = await _userManager.GetUserAsync(User);
        var isAdmin = User.IsInRole("SuperAdmin");

        var enrollmentCount = await _context.Enrollments.CountAsync(e => e.SubjectId == id);
        var isEnrolled = user != null && await _context.Enrollments.AnyAsync(e => e.UserId == user.Id && e.SubjectId == id);

        ViewBag.EnrollmentCount = enrollmentCount;
        ViewBag.IsEnrolled = isEnrolled;
        ViewBag.IsAdmin = isAdmin;

        return View(subject);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Enroll(int id)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return RedirectToAction("Login", "Account");

        var subject = await _context.Subjects.FindAsync(id);
        if (subject == null) return NotFound();

        var existing = await _context.Enrollments
            .FirstOrDefaultAsync(e => e.UserId == user.Id && e.SubjectId == id);

        if (existing != null)
        {
            TempData["Info"] = "You are already enrolled in this subject.";
            return RedirectToAction("Details", new { id });
        }

        var enrollment = new Enrollment
        {
            UserId = user.Id,
            SubjectId = id,
            EnrolledAt = DateTime.UtcNow
        };

        _context.Enrollments.Add(enrollment);
        await _context.SaveChangesAsync();

        TempData["Success"] = $"Successfully enrolled in {subject.Title}!";
        return RedirectToAction("Details", new { id });
    }

    [Authorize(Roles = "SuperAdmin")]
    public IActionResult Create()
    {
        ViewBag.Categories = new SelectList(new[] { "Coding", "Languages" });
        ViewBag.Levels = new SelectList(new[] { "Basic", "Intermediate", "Advanced" });
        return View();
    }

    [HttpPost]
    [Authorize(Roles = "SuperAdmin")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Subject subject)
    {
        if (ModelState.IsValid)
        {
            subject.CreatedAt = DateTime.UtcNow;
            _context.Subjects.Add(subject);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Subject created successfully!";
            return RedirectToAction("Index");
        }
        ViewBag.Categories = new SelectList(new[] { "Coding", "Languages" }, subject.Category);
        ViewBag.Levels = new SelectList(new[] { "Basic", "Intermediate", "Advanced" }, subject.Level);
        return View(subject);
    }

    [Authorize(Roles = "SuperAdmin")]
    public async Task<IActionResult> Edit(int id)
    {
        var subject = await _context.Subjects.FindAsync(id);
        if (subject == null) return NotFound();
        ViewBag.Categories = new SelectList(new[] { "Coding", "Languages" }, subject.Category);
        ViewBag.Levels = new SelectList(new[] { "Basic", "Intermediate", "Advanced" }, subject.Level);
        return View(subject);
    }

    [HttpPost]
    [Authorize(Roles = "SuperAdmin")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Subject subject)
    {
        if (ModelState.IsValid)
        {
            _context.Subjects.Update(subject);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Subject updated successfully!";
            return RedirectToAction("Index");
        }
        ViewBag.Categories = new SelectList(new[] { "Coding", "Languages" }, subject.Category);
        ViewBag.Levels = new SelectList(new[] { "Basic", "Intermediate", "Advanced" }, subject.Level);
        return View(subject);
    }

    [HttpPost]
    [Authorize(Roles = "SuperAdmin")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var subject = await _context.Subjects.FindAsync(id);
        if (subject != null)
        {
            _context.Subjects.Remove(subject);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Subject deleted successfully!";
        }
        return RedirectToAction("Index");
    }
}