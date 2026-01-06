namespace QuestionBank.Components.Model;

public class Flag
{
    public int Id { get; set; }
    public int QuestionId { get; set; }
    public FlagType FlagType { get; set; }
}

public enum FlagType
{
    None = 1,
    Simple = 2,
    ToRead = 3,
    Exceptional = 4
}