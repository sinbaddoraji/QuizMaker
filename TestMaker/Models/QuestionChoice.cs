namespace TestMaker.Models;

public class QuestionChoice
{
    public Guid QuestionChoiceId { get; set; } = Guid.NewGuid();

    public string? Content { get; set; }
}