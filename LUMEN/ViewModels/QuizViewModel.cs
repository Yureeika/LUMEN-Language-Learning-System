namespace LUMEN.ViewModels;

using LUMEN.Models;

public class QuizViewModel
{
    public Quiz Quiz { get; set; }
    public List<Question> Questions { get; set; } = [];
    public Dictionary<int, string> UserAnswers { get; set; } = [];
    public int TimeLimitMinutes { get; set; }
}

public class QuizSubmitViewModel
{
    public int QuizId { get; set; }
    public required Dictionary<int, string> Answers { get; set; } = [];
}
