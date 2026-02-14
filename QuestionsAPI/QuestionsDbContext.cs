using Microsoft.EntityFrameworkCore;

namespace QuestionsAPI
{
    public class QuestionsDbContext : DbContext
    {
        public QuestionsDbContext(DbContextOptions<QuestionsDbContext> options) : base(options) { }
        public DbSet<QuestionsAndAnswers> QuestionsAndAnswers { get; set; }
    }
}
