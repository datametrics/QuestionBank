using QuestionBank.Components.Model;

namespace QuestionBank.Components.Model;

public class TestRunModel
{
    public Guid RunId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public List<Item> Items { get; set; } = new();
    public TimeSpan Duration { get; set; }

    public string GetFormattedDuration()
    {
        if (!EndTime.HasValue) return string.Empty;
        
        Duration = EndTime.Value - StartTime;
        return $"{Duration.Minutes} min {Duration.Seconds} sec";
    }

    public int CorrectCount => Items.Count(x => x.CorrectAnswer == x.SelectedAnswer);

    public int IncorrectCount => Items.Count(x => x.CorrectAnswer != x.SelectedAnswer);

    public float PercentCorrect
    {
        get
        {
            if (Items.Count == 0) return 0;
            return (float)CorrectCount / Items.Count;
        }
    }
}