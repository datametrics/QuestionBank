using QuestionBank.Components.Model;

namespace QuestionBank.Components.Services;

public class ResultsManager(ConfigService configService, IHttpContextAccessor httpContextAccessor)
{
    public List<QuestionResultSet> SaveResults(TestRunModel testRun)
    {
        var newResultSet = CreateResultSet(testRun);
        
        configService.SaveResultSets(newResultSet);
        return configService.LoadResultSets();
    }

    private QuestionResultSet CreateResultSet(TestRunModel testRun)
    {
        var resultSet = new QuestionResultSet
        {
            TimeStamp = DateTime.Now,
            Start = testRun.StartTime,
            End = DateTime.Now,
            UserId = httpContextAccessor.HttpContext?.User.Identity?.Name  ?? "no-user"
        };

        foreach (var item in testRun.Items)
        {
            resultSet.QuestionResults.Add(new QuestionResult
            {
                Part = item.Part,
                Chapter = item.Chapter,
                QuestionId = item.Id,
                CorrectAnswerId = item.CorrectAnswer.GetValueOrDefault(),
                SelectedAnswerId = item.SelectedAnswer,
                Topic = item.Topic
            });
        }

        return resultSet;
    }
}
