namespace TestMaker.Models
{
	public class Quiz
	{
		public string Name {get; set; } = null!;

		public string Description { get; set; } = null!;

		public List<Question>? Questions { get; set; }
	}
}
