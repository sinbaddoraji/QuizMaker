namespace TestMaker.Models;

public class QuestionModel
{
	public Guid QuestionId { get; set; } = Guid.NewGuid();
	public string? Content { get; set; }
	public List<string> CorrectAnswerIndex { get; set; } = new List<string>();

	public List<string> Choices { get; set; } = new List<string>();

	public List<bool> ChoicesChecked { get; set; } = new List<bool>();
}