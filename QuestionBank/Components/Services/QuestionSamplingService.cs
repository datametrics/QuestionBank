using QuestionBank.Components.Model;

namespace QuestionBank.Components.Services;

public class SampleSettings
{
    public int NumberOfQuestions { get; set; }
    public Difficulty Difficulty { get; set; }
    public QuizStyle QuizStyle { get; set; }
    public SamplingMode SamplingMode { get; set; }
    public FlagType FlagType { get; set; }
    public List<Question> Questions { get; set; } = [];
    public List<QuestionResult> PreviousResults { get; set; } = [];
}

public class QuestionSampler(ConfigService configService)
{
    private readonly Random _random = new();

    public List<Item> Sample(SampleSettings settings)
    {
        var flags = configService.LoadFlags();

        var filteredQuestions = ApplyFilters(settings, flags);
        var sampledQuestions = ApplySamplingMode(filteredQuestions, settings);
        var randomizedQuestions = RandomizeQuestions(sampledQuestions);
        var finalQuestions = TakeRequiredNumber(randomizedQuestions, settings.NumberOfQuestions);

        return ConvertToItems(finalQuestions, flags);
    }

    private List<Question> ApplyFilters(SampleSettings settings, List<Flag> flags)
    {
        var questions = settings.Questions;
        questions = FilterByFlags(questions, flags, settings.FlagType);
        questions = FilterByDifficulty(questions, settings.Difficulty);
        return questions;
    }

    private List<Question> FilterByFlags(List<Question> questions, List<Flag> flags, FlagType flagType)
    {
        if (flagType == FlagType.None)
        {
            return questions;
        }

        return questions
            .Where(q => flags.Any(f => f.QuestionId == q.Id && f.FlagType == flagType))
            .ToList();
    }

    private List<Question> FilterByDifficulty(List<Question> questions, Difficulty difficulty)
    {
        if (difficulty == Difficulty.All)
        {
            return questions;
        }

        return questions.Where(q => q.Difficulty == difficulty).ToList();
    }

    private List<Question> ApplySamplingMode(List<Question> questions, SampleSettings settings)
    {
        return settings.SamplingMode switch
        {
            SamplingMode.AllButPreviouslyAnswered => GetUnansweredQuestions(questions, settings.PreviousResults),
            SamplingMode.PreviouslyIncorrect => GetIncorrectlyAnsweredQuestions(questions, settings.PreviousResults),
            SamplingMode.AllQuestions => questions,
            _ => questions
        };
    }

    private List<Question> GetUnansweredQuestions(List<Question> questions, List<QuestionResult> previousResults)
    {
        if (!previousResults.Any())
        {
            return questions;
        }

        return questions
            .Where(q => !WasQuestionPreviouslyAnswered(q, previousResults))
            .ToList();
    }

    private List<Question> GetIncorrectlyAnsweredQuestions(List<Question> questions, List<QuestionResult> previousResults)
    {
        if (!previousResults.Any())
        {
            return new List<Question>();
        }

        return questions
            .Where(q => WasQuestionAnsweredIncorrectly(q, previousResults))
            .ToList();
    }

    private bool WasQuestionPreviouslyAnswered(Question question, List<QuestionResult> previousResults)
    {
        return previousResults.Any(r =>
            r.Part == question.Part &&
            r.Chapter == question.Chapter &&
            r.Topic == question.Topic &&
            r.QuestionId == question.Id);
    }

    private bool WasQuestionAnsweredIncorrectly(Question question, List<QuestionResult> previousResults)
    {
        return previousResults.Any(r =>
            r.Part == question.Part &&
            r.Chapter == question.Chapter &&
            r.Topic == question.Topic &&
            r.QuestionId == question.Id &&
            r.CorrectAnswerId != r.SelectedAnswerId);
    }

    private List<Question> RandomizeQuestions(List<Question> questions)
    {
        return questions.OrderBy(_ => _random.Next()).ToList();
    }

    private List<Question> TakeRequiredNumber(List<Question> questions, int numberOfQuestions)
    {
        return questions.Take(numberOfQuestions).ToList();
    }

    private List<Item> ConvertToItems(List<Question> questions, List<Flag> flags)
    {
        return questions.Select(q => CreateItem(q, flags)).ToList();
    }

    private Item CreateItem(Question question, List<Flag> flags)
    {
        var item = new Item
        {
            Id = question.Id,
            BtId = question.BtId,
            Reference = question.Reference,
            Topic = question.Topic.GetValueOrDefault(),
            Chapter = question.Chapter.GetValueOrDefault(),
            LearningObjective = question.LearningObjective,
            QuestionBody = question.QuestionBody,
            CorrectAnswer = question.CorrectAnswerId,
            Explanation = question.Explanation,
            Part = question.Part,
            Answers = ConvertAnswers(question),
            SelectedAnswer = null
        };

        ApplyFlag(item, flags);
        return item;
    }

    private List<Response> ConvertAnswers(Question question)
    {

        return
        [
            new Response
            {
                Id = 1,
                Text = question.Answer1
            },
            new Response
            {
                Id = 2,
                Text = question.Answer2
            },
            new Response
            {
            Id = 3,
            Text = question.Answer3
            },
            new Response
            {
                Id = 4,
                Text = question.Answer4
            }
        ];
    }

    private void ApplyFlag(Item item, List<Flag> flags)
    {
        var flag = flags.FirstOrDefault(f => f.QuestionId == item.Id);
        if (flag != null)
        {
            item.FlagType = flag.FlagType;
        }
    }
}
