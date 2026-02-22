using QuestionsAPI.DTO;
using System.Linq;
using System.Collections.Generic;

namespace QuestionsAPI.Functions
{
    public static class QuestionsAnswersTransform
    {
        // Groups questions by Category and returns a dictionary suitable for JSON serialization
        public static CategoryWrapper ResponseTransform(List<QuestionsAndAnswers> questionsAndAnswers)
        {
            return new CategoryWrapper
            {
                categoryWrapper = questionsAndAnswers
                .GroupBy(q => q.Category)
                .ToDictionary(
                    g => g.Key ?? string.Empty,
                    g => g.Select(q => new QuestionAndAnswers
                    {
                        question = q.Text,
                        answers = (q.Options ?? string.Empty).Split(';').Where(s => !string.IsNullOrWhiteSpace(s)).ToList(),
                        correctAnswer = q.Answer
                    }).ToList()
                )
            };
        }
    }
}