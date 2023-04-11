namespace TestMaker.Models
{
	public class TestModel
	{
		public Guid id { get; set; }
		public List<TestMaker.Models.QuestionModel> Tests { get; set; } = new List<TestMaker.Models.QuestionModel>();
		public string? TestDescription { get; set; }
		public string? TestName { get; set; }
	}
}
