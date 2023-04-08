namespace TestMaker.Models
{
	public class Survey
	{
		public Guid SurveyId { get; set; }
		public string Name {get; set; } = null!;

		public string Description { get; set; } = null!;

		public List<Question>? Questions { get; set; }
	}
}
