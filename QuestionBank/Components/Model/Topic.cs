namespace QuestionBank.Components.Model;

public class Topic
{
    public int Id { get; set; }
    public int MapId { get; set; }
    public Part Part { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Authors { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<Chapter> Chapters { get; set; } = [];
}

public class Chapter
{
    public int Id { get; set; }
    
    public string DisplayId { get; set; } = string.Empty;
    public int DisplayOrder { get; set; } = 0;
    public int MapId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}
