namespace TestMaker.Models;

public class QuestionModel
{
	public Guid QuestionId { get; set; } = Guid.NewGuid();
	public string? Content { get; set; }
	public int CorrectAnswerIndex { get; set; }

	public List<string> Choices { get; set; } = new List<string>();

	public int SelectedIndex {get; set; }
}