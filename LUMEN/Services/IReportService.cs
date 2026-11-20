namespace LUMEN.Services;

using LUMEN.ViewModels;

public interface IReportService
{
    Task<ReportViewModel> GenerateStudentReportAsync(string userId, CancellationToken cancellationToken = default);
}