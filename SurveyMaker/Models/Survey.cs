
namespace TestMaker.Models
{
	public class Test
	{
		public Guid TestId { get; set; }
		public string? Name {get; set; }

		public string? Description { get; set; }

		public string? UserId { get; set; }

		public DateTime CreatedDate { get; set; } = DateTime.Now;
		
        public string Questions { get; set; } = System.Text.Json.JsonSerializer.Serialize(new List<Question>());

    }
}
