namespace LUMEN.Services;

using LUMEN.Data;
using LUMEN.ViewModels;
using Microsoft.EntityFrameworkCore;

public class ReportService(ApplicationDbContext context) : IReportService
{
    public async Task<ReportViewModel> GenerateStudentReportAsync(string userId, CancellationToken cancellationToken = default)
    {
        var user = await context.Users.FindAsync([userId], cancellationToken);
        if (user == null) throw new InvalidOperationException("User not found.");

        var enrolled = await context.Enrollments.CountAsync(e => e.UserId == userId, cancellationToken);
        var completed = await context.Enrollments.CountAsync(e => e.UserId == userId && e.IsCompleted, cancellationToken);
        var passed = await context.QuizAttempts.CountAsync(q => q.UserId == userId && q.Passed, cancellationToken);
        var failed = await context.QuizAttempts.CountAsync(q => q.UserId == userId && !q.Passed, cancellationToken);

        return new ReportViewModel
        {
            UserId = user.Id,
            UserName = $"{user.FirstName} {user.LastName}",
            TotalXP = user.TotalXP,
            CurrentLevel = int.TryParse(user.CurrentLevel, out var lvl) ? lvl : 0,
            SubjectsEnrolled = enrolled,
            SubjectsCompleted = completed,
            QuizzesPassed = passed,
            QuizzesFailed = failed,
            MonthlyXP = []
        };
    }
}
