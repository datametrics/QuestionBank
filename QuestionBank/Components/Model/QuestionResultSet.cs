using System.ComponentModel.DataAnnotations;

namespace QuestionBank.Components.Model;

public class QuestionResultSet
{
    public Guid Id { get; set; }
    public DateTime TimeStamp { get; set; }
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    
    [MaxLength(200)]
    public string UserId { get; set; } = string.Empty;
    
    public List<QuestionResult> QuestionResults { get; set; } = [];
}

public class QuestionStatistic
{
    public int QuestionId { get; set; }
    public int? Topic { get; set; }
    public int? Chapter { get; set; }
    public Part Part { get; set; }
    public int RetryCount { get; set; }
}