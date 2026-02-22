namespace QuestionsAPI.DTO
{
    public class QuestionAndAnswers
    {
        public string question { get; set; } = string.Empty;
        public List<string> answers { get; set; } = new List<string>();
        public string correctAnswer { get; set; } = string.Empty;
    }
}
