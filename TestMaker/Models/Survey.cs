namespace TestMaker.Models
{
	public class Survey
	{
		public Guid SurveyId { get; set; }
		public string? Name {get; set; }

		public string? Description { get; set; }

		public string? UserId { get; set; }

		public DateTime CreatedDate { get; set; } = DateTime.Now;

		public List<Question>? Questions { get; set; } = new List<Question>();
	}
	
}
