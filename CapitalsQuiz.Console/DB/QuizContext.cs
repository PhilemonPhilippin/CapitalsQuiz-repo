using CapitalsQuiz.Console.Entities;
using Microsoft.EntityFrameworkCore;

namespace CapitalsQuiz.Console.DB;

internal class QuizContext(DbContextOptions<QuizContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<AnswerHistory> Answers { get; set; }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<User>().HasIndex(u => u.Alias).IsUnique();
    }
}
