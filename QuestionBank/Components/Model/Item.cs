namespace QuestionBank.Components.Model;

public class Item
{
    public int Id { get; set; }
    public string? BtId { get; set; } = string.Empty;
    public string? Reference { get; set; } = string.Empty;
    public int Topic { get; set; }
    public int Chapter { get; set; }
    public string? LearningObjective { get; set; } = string.Empty;
    public string QuestionBody { get; set; } = string.Empty;
    public List<Response> Answers { get; set; } = new();
    
    public string? Explanation { get; set; }= string.Empty;
    public int? SelectedAnswer { get; set; }
    public int? FinalAnswer { get; set; }
    public int? CorrectAnswer { get; set; }
    
    public Part Part { get; set; }
    public FlagType FlagType { get; set; } = FlagType.None;

    public bool IsCorrectlyAnswered()
    {
        return CorrectAnswer == FinalAnswer;
    }
}