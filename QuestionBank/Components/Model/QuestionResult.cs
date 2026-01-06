namespace QuestionBank.Components.Model;

public class QuestionResult
{
    public int Id { get; set; }
    public int? Topic { get; set; }
    public int? Chapter { get; set; }
    public int QuestionId { get; set; }
    public int CorrectAnswerId { get; set; }
    public int? SelectedAnswerId { get; set; }
    public Part Part { get; set; }
    
    // Foreign key + navigation back to parent set
    public Guid QuestionResultSetId { get; set; }
    public QuestionResultSet QuestionResultSet { get; set; } = null!;

    public bool IsCorrect => CorrectAnswerId == SelectedAnswerId;
}