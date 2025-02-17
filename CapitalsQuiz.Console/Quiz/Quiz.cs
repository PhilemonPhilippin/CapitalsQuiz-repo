using CapitalsQuiz.Console.DB;
using CapitalsQuiz.Console.Entities;
using Microsoft.EntityFrameworkCore;

namespace CapitalsQuiz.Console.Quiz;

internal class Quiz(QuizContext context)
{
    internal async Task Run()
    {
        System.Console.WriteLine("Hello, and welcome in the CapitalsQuiz program.");

        string alias = AskAlias();
        User user = await GetUser(alias);

        Random random = new();

        List<CountryCapital> countryCapitals = CapitalsLoader.GetCountryCapitals();

        string answer;
        do
        {

            QuizSolution quizSolution = PickRandomCountryCapital(countryCapitals, random);
            await AskQuizQuestion(user, quizSolution);

            System.Console.WriteLine($"User points : {user.Points}");
            System.Console.WriteLine("""Do you want to quit the program ? Type "y" or "yes" to quit. Just press Enter to keep playing.""");
            answer = System.Console.ReadLine();
        } while (answer != "yes" && answer != "y");
    }

    internal string AskAlias()
    {
        string alias;
        do
        {
            System.Console.WriteLine("What is your alias ?");
            alias = System.Console.ReadLine();
            if (string.IsNullOrEmpty(alias)) System.Console.WriteLine("You must write a valid alias.");
        } while (string.IsNullOrEmpty(alias));

        System.Console.WriteLine("Your alias is " + alias);
        return alias;
    }

    internal async Task<User> GetUser(string alias)
    {
        User user = await context.Users.FirstOrDefaultAsync(u => u.Alias.ToLower() == alias.ToLower());
        if (user is null)
        {
            User newUser = new()
            {
                Alias = alias,
            };
            await context.Users.AddAsync(newUser);
            await context.SaveChangesAsync();
            return newUser;
        }
        else
        {
            return user;
        }
    }

    internal async Task AskQuizQuestion(User user, QuizSolution quizSolution)
    {
        System.Console.WriteLine($"Here comes the quiz question.");
        int points;
        string userAnswer;
        int answerTryNumber = 0;
        do
        {
            System.Console.WriteLine($"Try nb°{answerTryNumber}. What is the capital of {quizSolution.CountryName} ?");
            System.Console.WriteLine($"Hint : {quizSolution.CapitalName}");
            userAnswer = System.Console.ReadLine();
            answerTryNumber++;

            if (userAnswer != quizSolution.CapitalName)
            {
                System.Console.WriteLine("Wrong answer.");
                if (answerTryNumber < 3)
                {
                    await RegisterAnswer(quizSolution, userAnswer, answerTryNumber, 0, user);
                }
            };
        } while (userAnswer != quizSolution.CapitalName && answerTryNumber < 3);

        if (userAnswer == quizSolution.CapitalName)
        {
            System.Console.WriteLine("Good answer. Bravo!");
            points = answerTryNumber switch
            {
                1 => 5,
                2 => 3,
                _ => 1
            };
        }
        else
        {
            points = -2;
        }

        await UpdatePoints(user, points);
        await RegisterAnswer(quizSolution, userAnswer, answerTryNumber, points, user);
    }

    internal async Task RegisterAnswer(QuizSolution quizSolution, string userAnswer, int answerTryNumber, int points, User user)
    {
        AnswerHistory answer = new()
        {
            CountryName = quizSolution.CountryName,
            CapitalName = quizSolution.CapitalName,
            UserAnswer = userAnswer ?? string.Empty,
            AnswerTryNumber = answerTryNumber,
            IsRightAnswer = points == 5 || points == 3 || points == 1,
            AppliedPoints = points,
            UserId = user.Id,
            User = user
        };

        await context.Answers.AddAsync(answer);
        await context.SaveChangesAsync();
    }

    internal async Task UpdatePoints(User user, int points)
    {
        user.Points += points;
        user.ModifiedOn = DateTime.UtcNow;

        await context.SaveChangesAsync();
    }

    internal QuizSolution PickRandomCountryCapital(List<CountryCapital> countryCapitals, Random random)
    {
        int total = countryCapitals.Count;
        int offset = random.Next(0, total);

        CountryCapital countryCapital = countryCapitals[offset];

        return new QuizSolution()
        {
            CountryName = countryCapital.CountryName,
            CapitalName = countryCapital.CapitalName,
        };
    }
}
