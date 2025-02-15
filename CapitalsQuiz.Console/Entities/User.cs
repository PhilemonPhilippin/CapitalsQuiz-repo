using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapitalsQuiz.Console.Entities;

internal class User
{
    public Guid Id { get; set; }
    public string Alias { get; set; }
    public int Points { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime ModifiedOn { get; set; }
    public List<AnswerHistory> Answers { get; } = new();

    public User()
    {
        Id = Guid.NewGuid();
        CreatedOn = DateTime.UtcNow;
        ModifiedOn = DateTime.UtcNow;
    }
}
