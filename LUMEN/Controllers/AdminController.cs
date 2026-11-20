namespace LUMEN.Controllers;

using LUMEN.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Authorize(Roles = "SuperAdmin,Admin")]
public class AdminController(ApplicationDbContext context) : Controller
{
    public async Task<IActionResult> Dashboard()
    {
        var userCount = await context.Users.CountAsync();
        var subjectCount = await context.Subjects.CountAsync();
        var lessonCount = await context.Lessons.CountAsync();
        var quizCount = await context.Quizzes.CountAsync();
        var enrollmentCount = await context.Enrollments.CountAsync();
        var quizAttemptCount = await context.QuizAttempts.CountAsync();
        var chatMessageCount = await context.ChatMessages.CountAsync();
        var announcementCount = await context.Announcements.CountAsync();

        ViewBag.UserCount = userCount;
        ViewBag.SubjectCount = subjectCount;
        ViewBag.LessonCount = lessonCount;
        ViewBag.QuizCount = quizCount;
        ViewBag.EnrollmentCount = enrollmentCount;
        ViewBag.QuizAttemptCount = quizAttemptCount;
        ViewBag.ChatMessageCount = chatMessageCount;
        ViewBag.AnnouncementCount = announcementCount;

        return View();
    }

    public IActionResult ManageSubjects() => RedirectToAction("Index", "Subjects");
    public IActionResult ManageLessons() => RedirectToAction("Index", "Lessons");
    public IActionResult ManageQuizzes() => RedirectToAction("Index", "DailyQuiz");
    public IActionResult ManageUsers() => RedirectToAction("Index", "UserManagement");
    public IActionResult ManageRoles() => RedirectToAction("Index", "UserManagement");
    public IActionResult ManageAnnouncements() => RedirectToAction("Index", "Announcements");
    public IActionResult AuditLog() => RedirectToAction("Index", "AuditLog");
}