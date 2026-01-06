using MudBlazor;
using QuestionBank.Components.Model;

namespace QuestionBank.Components;

public class StatisticsGenerator(List<Question> questions)
{
    public List<TreeItemData<string>> GenerateStatisticsTree(List<QuestionResultSet> resultSets, List<Topic> topics)
    {
        var distinctResults = GetDistinctQuestionResults(resultSets);
        var statisticsItems = new List<TreeItemData<string>>();

        foreach (var partGroup in distinctResults.GroupBy(x => x.Part))
        {
            var partItem = CreatePartStatisticsItem(partGroup, topics);
            statisticsItems.Add(partItem);
        }

        return statisticsItems;
    }

    private List<QuestionResult> GetDistinctQuestionResults(List<QuestionResultSet> resultSets)
    {
        return resultSets
            .SelectMany(x => x.QuestionResults)
            .GroupBy(q => q.QuestionId)
            .Select(g => g.Last())
            .ToList();
    }

    public List<QuestionStatistic> GetQuestionStatistics(List<QuestionResultSet> resultSets)
    {
        return resultSets.SelectMany(x => x.QuestionResults)
            .Where(x => x.Part == Part.Part2)
            .GroupBy(q => q.QuestionId)
            .Select(group => new QuestionStatistic()
            {
                QuestionId = group.Key, 
                Part = group.First().Part,
                RetryCount = group.Count(),
                Chapter = group.Select(x=>x.Chapter).Distinct().Single(),
                Topic = group.Select(x=>x.Topic).Distinct().Single()
            })
            .OrderByDescending(x => x.RetryCount)
            .ToList();
    }

    public (List<ChartSeries> Series, string[] Labels) BuildChartSeries(List<QuestionResultSet> resultSets)
    {
        // Collect all distinct topics across all sets
        var labels = resultSets
            .SelectMany(rs => rs.QuestionResults)
            .Where(x=>x.Part == Part.Part2)
            .Select(q => q.Topic)
            .Distinct()
            .OrderBy(t => t)
            .Select(t => $"Topic {t}")
            .ToArray();

        // Group by Part and build one ChartSeries per Part
        var series = resultSets
            .SelectMany(rs => rs.QuestionResults)
            .Where(x=>x.Part == Part.Part2)
            .GroupBy(q => q.Part)
            .Select(partGroup => new ChartSeries
            {
                Name = partGroup.Key.ToString(),
                Data = labels.Select(label =>
                {
                    var topicNum = int.Parse(label.Replace("Topic ", ""));
                    var topicGroup = partGroup.Where(q => q.Topic == topicNum).ToList();
                    if (topicGroup.Count == 0) return 0d;

                    var correct = topicGroup.Count(q => q.IsCorrect);
                    return (double)correct / topicGroup.Count * 100;
                }).ToArray()
            })
            .ToList();

        return (series, labels);
    }

    private TreeItemPresenter CreatePartStatisticsItem(IGrouping<Part, QuestionResult> partGroup, List<Topic> topics)
    {
        var totalInPart = questions.Distinct().Count(x => x.Part == partGroup.Key);
        var correctPercentage = CalculateCorrectPercentage(partGroup);
        
        var partItem = new TreeItemPresenter(
            text: partGroup.Key.ToString(),
            icon: Icons.Material.Filled.Label,
            correctPercentage: correctPercentage,
            processedCount: partGroup.Count(),
            totalCount: totalInPart)
        {
            Value = partGroup.Key.ToString(),
            Children = []
        };

        foreach (var topicGroup in partGroup.GroupBy(q => q.Topic).OrderBy(x => x.Key))
        {
            var topic = topics.SingleOrDefault(x=>x.MapId == topicGroup.Key);
            var topicItem = CreateTopicStatisticsItem(topicGroup, partGroup.Key, topic);
            partItem.Children.Add(topicItem);
        }

        partItem.Expanded = true;
        return partItem;
    }

    private TreeItemPresenter CreateTopicStatisticsItem(IGrouping<int?, QuestionResult> topicGroup, Part part,
        Topic? topic)
    {
        var totalInTopic = questions.Distinct()
            .Count(x => x.Topic == topicGroup.Key && x.Part == part);
        var correctPercentage = CalculateCorrectPercentage(topicGroup);

        var topicItem = new TreeItemPresenter(
            text: $"Topic {topicGroup.Key.GetValueOrDefault()} - {topic?.Name}",
            icon: Icons.Material.Filled.QuestionAnswer,
            correctPercentage: correctPercentage,
            processedCount: topicGroup.Count(),
            totalCount: totalInTopic)
        {
            Value = $"{part}|{topicGroup.Key}",
            Children = []
        };

        foreach (var chapterGroup in topicGroup.GroupBy(q => q.Chapter).OrderBy(x => x.Key))
        {
            var chapter = topic?.Chapters.SingleOrDefault(x => x.MapId == chapterGroup.Key);
            var chapterItem = CreateChapterStatisticsItem(chapterGroup, part, topicGroup.Key, chapter);
            topicItem.Children.Add(chapterItem);
        }

        return topicItem;
    }

    private TreeItemPresenter CreateChapterStatisticsItem(IGrouping<int?, QuestionResult> chapterGroup, Part part,
        int? topic, Chapter? chapterInfo)
    {
        var totalInChapter = questions.Distinct()
            .Count(x => x.Topic == topic && x.Part == part && x.Chapter == chapterGroup.Key);
        var correctPercentage = CalculateCorrectPercentage(chapterGroup);

        return new TreeItemPresenter(
            text: $"{chapterInfo?.DisplayId} {chapterInfo?.Name}",
            icon: Icons.Material.Filled.Book,
            correctPercentage: correctPercentage,
            processedCount: chapterGroup.Count(),
            totalCount: totalInChapter)
        {
            Value = $"{part}|{topic}|{chapterGroup.Key}",
            Children = []
        };
    }

    private float CalculateCorrectPercentage(IEnumerable<QuestionResult> results)
    {
        var resultsList = results.ToList();
        if (resultsList.Count == 0) return 0;

        var correctCount = resultsList.Count(x => x.IsCorrect);
        return (float)correctCount / resultsList.Count;
    }
}
