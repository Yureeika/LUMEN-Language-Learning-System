namespace LUMEN.Models;

public class CodeExecutionRequest
{
    public string Language { get; set; } = "python";

    public string Code { get; set; } = "";

    public string Input { get; set; } = "";
}