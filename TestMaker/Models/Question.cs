namespace TestMaker.Models;

public class Question
{
	public Guid QuestionId { get; set; }
	public string Content { get; set; } = null!;
	public string CorrectAnswerIndex { get; set; } = null!;

	public string[] Choices { get; set; } = null!;
}
