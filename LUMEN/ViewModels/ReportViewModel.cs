namespace LUMEN.ViewModels;

public class ReportViewModel
{
    public string UserId { get; set; }
    public string UserName { get; set; }
    public int TotalXP { get; set; }
    public int CurrentLevel { get; set; }
    public int SubjectsEnrolled { get; set; }
    public int SubjectsCompleted { get; set; }
    public int QuizzesPassed { get; set; }
    public int QuizzesFailed { get; set; }
    public List<MonthlyXPData> MonthlyXP { get; set; } = [];
}

public class MonthlyXPData
{
    public string Month { get; set; }
    public int XP { get; set; }
}
