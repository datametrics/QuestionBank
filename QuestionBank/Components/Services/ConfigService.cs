using Microsoft.EntityFrameworkCore;
using QuestionBank.Components.Model;

namespace QuestionBank.Components.Services;

public class ConfigService(QuestionBankContext dbContext, IHttpContextAccessor httpContextAccessor, ILogger<ConfigService> logger)
{
    public List<QuestionResultSet> LoadResultSets()
    {
        return LoadResultSetsImpl();
    }
    public void SaveResultSets(QuestionResultSet set)
    {
        SaveResultSetsImpl(set);
    }
    
    public List<Flag> LoadFlags()
    {
        return LoadFlagsImpl();
    }

    public void SaveFlags(List<Item> items)
    {
        SaveFlagsImpl(items);
    }
    
    public List<Question> LoadConfig()
    {
        return LoadConfigImpl();
    }

    public async Task SaveQuestionsAsync(List<Question> questions)
    {
        await SaveQuestionsAsyncImpl(questions);
    }
    public async Task SaveQuestionAsync(Question questions)
    {
        await SaveQuestionsAsyncImpl([questions]);
    }
    
     private List<Flag> LoadFlagsImpl()
    {
        return dbContext.Flags.ToList();
    }
    private void SaveFlagsImpl(List<Item> items)
    {
        foreach (var item in items)
        {
            var existingFlag = dbContext.Flags.FirstOrDefault(x => x.QuestionId == item.Id);

            if (existingFlag != null)
            {
                if (item.FlagType != FlagType.None)
                {
                    existingFlag.FlagType = item.FlagType;
                }
                else
                {
                    dbContext.Flags.Remove(existingFlag);
                }
            }
            else if (item.FlagType != FlagType.None)
            {
                dbContext.Flags.Add(new Flag
                {
                    FlagType = item.FlagType,
                    QuestionId = item.Id
                });
            }
        }

        dbContext.SaveChanges();
    }
    private List<Question> LoadConfigImpl()
    {
        return dbContext.Questions.ToList();
    }

    public List<Topic> LoadTopics()
    {
        return dbContext.Topics.Include(x=>x.Chapters).ToList();
    }

    private async Task SaveQuestionsAsyncImpl(List<Question> questions)
    {
        foreach (var question in questions)
        {
            var existing = await dbContext.Questions
                .FirstOrDefaultAsync(q => q.Id == question.Id);

            if (existing != null)
            {
                // Update existing entity
                dbContext.Entry(existing).CurrentValues.SetValues(question);
            }
            else
            {
                // Insert new entity
                await dbContext.Questions.AddAsync(question);
            }
        }

        await dbContext.SaveChangesAsync();
    }
    
    private List<QuestionResultSet> LoadResultSetsImpl()
    {
        return dbContext.ResultSets.Include(x=>x.QuestionResults).ToList();
    }

    private void SaveResultSetsImpl(QuestionResultSet set)
    {
        var existing = dbContext.ResultSets.FirstOrDefault(x => x.Id == set.Id);
        if (existing != null)
        {
            // update existing
            dbContext.Entry(existing).CurrentValues.SetValues(set);
        }
        else
        {
            // insert new
            dbContext.ResultSets.Add(set);
        }

        dbContext.SaveChanges();
    }

    public async Task DeleteResultSetsAsync()
    {
        var userId = httpContextAccessor.HttpContext?.User.Identity?.Name ?? "no-user";
        logger.LogWarning("Deleting question results for user:  {userId}", userId);
        await dbContext.Database.ExecuteSqlRawAsync("DELETE FROM ResultSets WHERE UserId = {0}", userId);
    }
}