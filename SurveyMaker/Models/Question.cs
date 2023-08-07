using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace TestMaker.Models
{
    public class Question
    {
        [Key]
        public Guid QuestionId { get; set; }

        public string QuestionContent { get; set; }

        public List<string> Answers { get; set; }

        public List<bool> AnswersState { get; set; }
    }

}
