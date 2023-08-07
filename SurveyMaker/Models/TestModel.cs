namespace TestMaker.Models
{
	public class TestModel
	{
		public Guid id { get; set; }
		public List<TestMaker.Models.Question> Tests { get; set; } = new();
		public string? TestDescription { get; set; }
		public string? TestName { get; set; }
	}
}
