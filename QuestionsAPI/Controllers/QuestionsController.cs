using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using QuestionsAPI.Functions;
namespace QuestionsAPI.Controllers
{
    [ApiController]
    [Route("questions")]
    [Authorize]
    public class QuestionsController : ControllerBase
    {
        private readonly QuestionsDbContext _context;

        public QuestionsController(QuestionsDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _context.QuestionsAndAnswers
                .Select(q => new QuestionsAndAnswers
                {
                    Id = q.Id,
                    Text = q.Text,
                    Answer = q.Answer,
                    Options = q.Options,
                    Category = q.Category
                })
                .ToListAsync();

            var transFormedResult = QuestionsAnswersTransform.ResponseTransform(result);

            return Ok(transFormedResult.categoryWrapper);
        }
    }
}
