using QuestionBank.Components.Model;

namespace QuestionBank.Components.Services;

public class QuestionSelector
{
    public List<Question> GetSelectedQuestions(IEnumerable<string> selectedValues, List<Question> allQuestions)
    {
        var results = new List<Question>();

        foreach (var selectedValue in selectedValues)
        {
            var selection = ParseSelection(selectedValue);
            if (selection == null) continue;

            var questions = FilterQuestions(selection.Value, allQuestions);
            results.AddRange(questions);
        }

        return results;
    }

    private (Part Part, int Topic, int Chapter)? ParseSelection(string value)
    {
        var parts = value.Split('|');
        if (parts.Length != 3) return null;

        if (!Enum.TryParse<Part>(parts[0], out var part)) return null;
        if (!int.TryParse(parts[1], out var topic)) return null;
        if (!int.TryParse(parts[2], out var chapter)) return null;

        return (part, topic, chapter);
    }

    private List<Question> FilterQuestions((Part Part, int Topic, int Chapter) selection, List<Question> questions)
    {
        return questions
            .Where(x => x.Part == selection.Part &&
                       x.Topic == selection.Topic &&
                       x.Chapter == selection.Chapter)
            .ToList();
    }
}
