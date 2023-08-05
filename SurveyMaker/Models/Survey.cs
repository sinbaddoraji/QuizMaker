
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


    public class TestWrapper
    {
        public Guid TestId { get; set; }
        public string? Name { get; set; }

        public string? Description { get; set; }

        public string? UserId { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public List<Question> Questions { get; set; }

        public void RemoveLastQuestion()
        {
            if (Questions != null && Questions.Any())
            {
                Questions.RemoveAt(Questions.Count - 1);
            }
        }

        public void AddOptionToQuestion(int questionIndex, string option = "")
        {
            if (Questions != null && Questions.Count > questionIndex)
            {
                Questions[questionIndex].Answers.Add(option);
            }
        }

        public void RemoveOptionFromQuestion(int questionIndex, int optionIndex)
        {
            if (Questions != null && Questions.Count > questionIndex)
            {
                var question = Questions[questionIndex];
                if (question.Answers != null && question.Answers.Count > optionIndex)
                {
                    question.Answers.RemoveAt(optionIndex);
                }
            }
        }
    }

}
