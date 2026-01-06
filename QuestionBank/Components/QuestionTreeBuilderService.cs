using MudBlazor;
using QuestionBank.Components.Model;

namespace QuestionBank.Components;

public class QuestionTreeBuilder
{
    public List<TreeItemData<string>> BuildQuestionSelectionTree(List<Question> questions, List<Topic> topics)
    {
        var treeItems = new List<TreeItemData<string>>();

        foreach (var partGroup in questions.GroupBy(x => x.Part))
        {
            var partItem = CreatePartTreeItem(partGroup, topics);
            treeItems.Add(partItem);
        }

        return treeItems;
    }

    private TreeItemPresenter CreatePartTreeItem(IGrouping<Part, Question> partGroup, List<Topic> topics)
    {
        var partItem = new TreeItemPresenter(
            text: partGroup.Key.ToString(),
            icon: Icons.Material.Filled.Label,
            questionCount: partGroup.Count())
        {
            Value = partGroup.Key.ToString(),
            Children = []
        };

        foreach (var topicGroup in partGroup.GroupBy(q => q.Topic).OrderBy(x => x.Key))
        {
            var topicInfo = topics.SingleOrDefault(x => x.MapId == topicGroup.Key);
            var topicItem = CreateTopicTreeItem(topicGroup, partGroup.Key, topicInfo);
            partItem.Children.Add(topicItem);
        }

        partItem.Expanded = true;
        return partItem;
    }

    private TreeItemPresenter CreateTopicTreeItem(IGrouping<int?, Question> topicGroup, Part part, Topic? topicInfo)
    {
        var topicItem = new TreeItemPresenter(
            text: $"Topic {topicGroup.Key.GetValueOrDefault()} - {topicInfo?.Name}",
            icon: Icons.Material.Filled.QuestionAnswer,
            questionCount: topicGroup.Count())
        {
            Value = $"{part}|{topicGroup.Key}",
            Children = []
        };

        foreach (var chapterGroup in topicGroup.GroupBy(q => q.Chapter).OrderBy(x => x.Key))
        {
            var chapterInfo = topicInfo?.Chapters.SingleOrDefault(x => x.MapId == chapterGroup.Key);
            var chapterItem = CreateChapterTreeItem(chapterGroup, part, topicGroup.Key, chapterInfo);
            topicItem.Children.Add(chapterItem);
        }

        topicItem.Children = topicItem.Children.Cast<TreeItemPresenter>().OrderBy(x => x.Order)
            .Cast<TreeItemData<string>>().ToList();
        return topicItem;
    }

    private TreeItemPresenter CreateChapterTreeItem(IGrouping<int?, Question> chapterGroup, Part part, int? topic,
        Chapter? chapterInfo)
    {
        return new TreeItemPresenter(
            text: $"{chapterInfo?.DisplayId} {chapterInfo?.Name}",
            icon: Icons.Material.Filled.Book,
            questionCount: chapterGroup.Count(),
            order:chapterInfo?.DisplayOrder)
        {
            Value = $"{part}|{topic}|{chapterGroup.Key}"
        };
    }
}
