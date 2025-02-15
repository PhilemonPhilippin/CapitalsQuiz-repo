using CapitalsQuiz.Console.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapitalsQuiz.Console.DB;

internal class QuizContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<AnswerHistory> Answers { get; set; }
    public QuizContext(DbContextOptions<QuizContext> options) : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<User>().HasIndex(u => u.Alias).IsUnique();
    }
}
