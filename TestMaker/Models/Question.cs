namespace TestMaker.Models;

public class Question
{
	public Guid QuestionId { get; set; } = Guid.NewGuid();
	public string? Content { get; set; }
	public string? CorrectAnswerIndex { get; set; }

	public List<QuestionChoice> Choices { get; set; } = new List<QuestionChoice>();
}