using System.ComponentModel.DataAnnotations.Schema;

namespace CapitalsQuiz.Console.Entities;

internal class AnswerHistory
{
    public Guid Id { get; set; }
    public string CountryName { get; set; }
    public string CapitalName { get; set; }
    public string UserAnswer { get; set; }
    public int AnswerTryNumber { get; set; }
    public bool IsRightAnswer { get; set; }
    public int AppliedPoints { get; set; }
    public DateTime CreatedOn { get; set; }

    [ForeignKey(nameof(User))]
    public Guid UserId { get; set; }
    public User User { get; set; }

    public AnswerHistory()
    {
        Id = Guid.NewGuid();
        CreatedOn = DateTime.UtcNow;
    }
}
