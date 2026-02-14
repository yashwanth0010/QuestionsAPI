using System.ComponentModel.DataAnnotations;

namespace QuestionsAPI
{
    public class QuestionsAndAnswers
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public required string Text { get; set; }
        [Required]
        public required string Answer { get; set; }

    }
}
