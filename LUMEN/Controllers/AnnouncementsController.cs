using LUMEN.Data;
using LUMEN.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LUMEN.Controllers
{
    [Authorize]
    public class AnnouncementsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AnnouncementsController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // =========================================================
        // ANNOUNCEMENT LIST
        // =========================================================
        // All authenticated users, including Students, can view
        // announcements.

        public async Task<IActionResult> Index()
        {
            var announcements = await _context.Announcements
                .Include(a => a.CreatedBy)
                .Where(a => a.IsActive)
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();

            return View(announcements);
        }

        // =========================================================
        // CREATE PAGE
        // =========================================================
        // Only SuperAdmin can create announcements.

        [Authorize(Roles = "SuperAdmin")]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // =========================================================
        // CREATE ANNOUNCEMENT
        // =========================================================

        [Authorize(Roles = "SuperAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Announcement announcement)
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return Unauthorized();
            }

            // Remove values that should be controlled by the server
            ModelState.Remove(nameof(Announcement.CreatedById));
            ModelState.Remove(nameof(Announcement.CreatedAt));
            ModelState.Remove(nameof(Announcement.CreatedBy));

            if (!ModelState.IsValid)
            {
                return View(announcement);
            }

            announcement.CreatedById = user.Id;
            announcement.CreatedAt = DateTime.UtcNow;

            _context.Announcements.Add(announcement);

            await _context.SaveChangesAsync();

            // =====================================================
            // SEND NOTIFICATION TO ALL OTHER USERS
            // =====================================================

            var allUsers = await _userManager.Users.ToListAsync();

            foreach (var u in allUsers)
            {
                // Don't send the notification to the person
                // who created the announcement.
                if (u.Id == user.Id)
                {
                    continue;
                }

                _context.Notifications.Add(new Notification
                {
                    UserId = u.Id,
                    Title = "New Announcement",
                    Message = announcement.Title,
                    Link = "/Announcements",
                    CreatedAt = DateTime.UtcNow
                });
            }

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] =
                "Announcement created and notifications sent!";

            return RedirectToAction(nameof(Index));
        }

        // =========================================================
        // TOGGLE ACTIVE / INACTIVE
        // =========================================================

        [Authorize(Roles = "SuperAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleActive(int id)
        {
            var announcement =
                await _context.Announcements.FindAsync(id);

            if (announcement == null)
            {
                TempData["ErrorMessage"] =
                    "Announcement not found.";

                return RedirectToAction(nameof(Index));
            }

            announcement.IsActive = !announcement.IsActive;

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] =
                announcement.IsActive
                    ? "Announcement activated."
                    : "Announcement deactivated.";

            return RedirectToAction(nameof(Index));
        }
    }
}