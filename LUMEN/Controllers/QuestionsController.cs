namespace LUMEN.Controllers;

using LUMEN.Data;
using LUMEN.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Authorize(Roles = "SuperAdmin")]
public class QuestionsController(ApplicationDbContext context) : Controller
{
    // =========================
    // QUESTIONS FOR A QUIZ
    // =========================
    [HttpGet]
    public async Task<IActionResult> Index(int quizId)
    {
        var quiz = await context.Quizzes
            .Include(q => q.Lesson)
            .FirstOrDefaultAsync(q => q.Id == quizId);

        if (quiz == null)
            return NotFound();

        var questions = await context.Questions
            .Where(q => q.QuizId == quizId)
            .OrderBy(q => q.Id)
            .ToListAsync();

        ViewBag.Quiz = quiz;

        return View(questions);
    }

    // =========================
    // CREATE QUESTION - GET
    // =========================
    [HttpGet]
    public async Task<IActionResult> Create(int quizId)
    {
        var quiz = await context.Quizzes
            .Include(q => q.Lesson)
            .FirstOrDefaultAsync(q => q.Id == quizId);

        if (quiz == null)
            return NotFound();

        ViewBag.Quiz = quiz;

        return View(new Question
        {
            QuizId = quizId,
            QuestionText = "",
            OptionA = "",
            OptionB = "",
            OptionC = "",
            OptionD = "",
            CorrectAnswer = "A",
            Points = 1
        });
    }

    // =========================
    // CREATE QUESTION - POST
    // =========================
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Question question)
    {
        var quiz = await context.Quizzes
            .Include(q => q.Lesson)
            .FirstOrDefaultAsync(q => q.Id == question.QuizId);

        if (quiz == null)
            return NotFound();

        // Remove any automatically generated validation
        // for the navigation property.
        ModelState.Remove(nameof(question.Quiz));

        // =========================
        // VALIDATE QUESTION
        // =========================

        if (string.IsNullOrWhiteSpace(question.QuestionText))
        {
            ModelState.AddModelError(
                nameof(question.QuestionText),
                "Please enter the question."
            );
        }

        if (string.IsNullOrWhiteSpace(question.OptionA))
        {
            ModelState.AddModelError(
                nameof(question.OptionA),
                "Please enter option A."
            );
        }

        if (string.IsNullOrWhiteSpace(question.OptionB))
        {
            ModelState.AddModelError(
                nameof(question.OptionB),
                "Please enter option B."
            );
        }

        if (string.IsNullOrWhiteSpace(question.OptionC))
        {
            ModelState.AddModelError(
                nameof(question.OptionC),
                "Please enter option C."
            );
        }

        if (string.IsNullOrWhiteSpace(question.OptionD))
        {
            ModelState.AddModelError(
                nameof(question.OptionD),
                "Please enter option D."
            );
        }

        // =========================
        // VALIDATE CORRECT ANSWER
        // =========================

        var validAnswers = new[] { "A", "B", "C", "D" };

        question.CorrectAnswer =
            question.CorrectAnswer?.Trim().ToUpper() ?? "";

        if (!validAnswers.Contains(question.CorrectAnswer))
        {
            ModelState.AddModelError(
                nameof(question.CorrectAnswer),
                "Please select A, B, C, or D as the correct answer."
            );
        }

        // =========================
        // VALIDATE POINTS
        // =========================

        if (question.Points < 1)
        {
            ModelState.AddModelError(
                nameof(question.Points),
                "Points must be at least 1."
            );
        }

        if (question.Points > 100)
        {
            ModelState.AddModelError(
                nameof(question.Points),
                "Points cannot be greater than 100."
            );
        }

        // =========================
        // SAVE QUESTION
        // =========================

        if (ModelState.IsValid)
        {
            context.Questions.Add(question);

            await context.SaveChangesAsync();

            TempData["SuccessMessage"] =
                "Question added successfully.";

            return RedirectToAction(
                nameof(Index),
                new { quizId = question.QuizId }
            );
        }

        // If validation fails, show the same page
        // with the validation messages.
        ViewBag.Quiz = quiz;

        return View(question);
    }

    // =========================
    // DELETE QUESTION
    // =========================
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var question = await context.Questions
            .FirstOrDefaultAsync(q => q.Id == id);

        if (question == null)
            return NotFound();

        var quizId = question.QuizId;

        context.Questions.Remove(question);

        await context.SaveChangesAsync();

        TempData["SuccessMessage"] =
            "Question deleted successfully.";

        return RedirectToAction(
            nameof(Index),
            new { quizId }
        );
    }
}