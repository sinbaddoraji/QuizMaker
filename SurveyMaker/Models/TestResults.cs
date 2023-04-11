namespace TestMaker.Models
{
	public class TestResults
	{
		public int Id { get; set; }

		public string? UserId { get; set; }

		public string? TestName { get; set; }

		public string? TestDescription { get; set; }

		public Guid TestId { get; set; }

		public int Score { get; set; }

		public DateTime DateTaken { get; set; } = DateTime.Now;
	}
}
