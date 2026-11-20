namespace LUMEN.Controllers;

using LUMEN.Data;
using LUMEN.Models;
using LUMEN.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

[Authorize]
public class DailyQuizController(ApplicationDbContext context, UserManager<ApplicationUser> userManager) : Controller
{
    public async Task<IActionResult> Index()
    {
        var dayOfYear = DateTime.UtcNow.DayOfYear;
        var quizzes = await context.Quizzes
            .Include(q => q.Lesson)
            .ThenInclude(l => l.Subject)
            .Where(q => q.IsPublished)
            .ToListAsync();

        // Rotate quizzes daily - show 3 different ones each day
        var todayQuizzes = new List<Quiz>();
        if (quizzes.Any())
        {
            var startIndex = dayOfYear % quizzes.Count;
            for (int i = 0; i < Math.Min(3, quizzes.Count); i++)
            {
                todayQuizzes.Add(quizzes[(startIndex + i) % quizzes.Count]);
            }
        }

        ViewBag.TodayDate = DateTime.UtcNow.ToString("dddd, MMMM dd, yyyy");
        return View(todayQuizzes);
    }

    public async Task<IActionResult> Start(int id)
    {
        var quiz = await context.Quizzes
            .Include(q => q.Questions)
            .FirstOrDefaultAsync(q => q.Id == id);

        if (quiz == null) return NotFound();

        var viewModel = new QuizViewModel
        {
            Quiz = quiz,
            Questions = quiz.Questions.ToList(),
            TimeLimitMinutes = quiz.TimeLimitMinutes
        };

        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Submit(QuizSubmitViewModel model)
    {
        var user = await userManager.GetUserAsync(User);
        if (user == null) return Unauthorized();

        var quiz = await context.Quizzes.Include(q => q.Questions).FirstOrDefaultAsync(q => q.Id == model.QuizId);
        if (quiz == null) return NotFound();

        int score = 0;
        int totalPoints = 0;
        foreach (var question in quiz.Questions)
        {
            totalPoints += question.Points;
            if (model.Answers.TryGetValue(question.Id, out var answer) && answer == question.CorrectAnswer)
            {
                score += question.Points;
            }
        }

        var attempt = new QuizAttempt
        {
            QuizId = quiz.Id,
            UserId = user.Id,
            Score = score,
            TotalPoints = totalPoints,
            Passed = totalPoints > 0
    && ((double)score / totalPoints * 100) >= quiz.PassingScore,
            StartedAt = DateTime.UtcNow,
            CompletedAt = DateTime.UtcNow,
            AnswersJson = JsonSerializer.Serialize(model.Answers)
        };

        context.QuizAttempts.Add(attempt);

        if (attempt.Passed)
        {
            user.TotalXP += score * 10;
            context.XPLogs.Add(new XPLog { UserId = user.Id, Amount = score * 10, Reason = $"Passed Quiz: {quiz.Title}" });
        }

        await context.SaveChangesAsync();
        return RedirectToAction("Result", new { id = attempt.Id });
    }

    public async Task<IActionResult> Result(int id)
    {
        var attempt = await context.QuizAttempts
            .Include(a => a.Quiz)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (attempt == null) return NotFound();
        return View(attempt);
    }
}

