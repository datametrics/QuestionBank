using QuestionBank.Components.Services;

namespace QuestionBank.Components.Model;

public class Question : IHasTimestamps
{
    public int Id { get; set; }
    public Part Part { get; set; }
    public string? BtId { get; set; } = string.Empty;
    public string? Reference { get; set; } = string.Empty;
    public int? Topic { get; set; }
    public int? Chapter { get; set; }
    public string? LearningObjective { get; set; } = string.Empty;
    public int CorrectAnswerId { get; set; }
    public string Explanation { get; set; }= string.Empty;
    public string QuestionBody { get; set; } = string.Empty;
    
    // Four fixed answers
    public string Answer1 { get; set; } = string.Empty;
    public string Answer2 { get; set; } = string.Empty;
    public string Answer3 { get; set; } = string.Empty;
    public string Answer4 { get; set; } = string.Empty;
    
    public Difficulty Difficulty { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class QuestionDto
{
    public string QuestionBody { get; set; } = string.Empty;
    public string Answer1 { get; set; } = string.Empty;
    public string Answer2 { get; set; } = string.Empty;
    public string Answer3 { get; set; } = string.Empty;
    public string Answer4 { get; set; } = string.Empty;
    public string Explanation { get; set; } = string.Empty;
    public int CorrectAnswerId { get; set; } = 0;
}
