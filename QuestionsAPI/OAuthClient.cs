using System.ComponentModel.DataAnnotations;

namespace QuestionsAPI
{
    public class OAuthClient
    {
        [Required]
        public required string ClientId { get; set; }
        [Required]
        public required string ClientSecret { get; set; }
    }
}
