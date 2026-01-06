using MudBlazor;

namespace QuestionBank.Components;

public class TreeItemPresenter : TreeItemData<string>
{
    public int? QuestionCount { get; set; }
    public float? CorrectPercentage { get; set; }
    public int? ProcessedCount { get; set; }
    public int? TotalCount { get; set; }

    public int? Order { get; set; }

    public TreeItemPresenter(
        string text, 
        string icon, 
        int? questionCount = null, 
        float? correctPercentage = null, 
        int? processedCount = null, 
        int? totalCount = null,
        int? order = null) : base(text)
    {
        Text = text;
        Icon = icon;
        QuestionCount = questionCount;
        CorrectPercentage = correctPercentage;
        ProcessedCount = processedCount;
        TotalCount = totalCount;
        Order = order;
    }
}
