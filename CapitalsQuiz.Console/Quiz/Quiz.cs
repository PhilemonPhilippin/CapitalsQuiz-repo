using CapitalsQuiz.Console.Entities;

namespace CapitalsQuiz.Console.Quiz;

internal class Quiz
{
    internal class QuizSolution
    {
        public string CountryName { get; set; }
        public string CapitalName { get; set; }
    }

    internal async Task Run()
    {
        System.Console.WriteLine("Hello, and welcome in the CapitalsQuiz program.");
        string alias = AskAlias();
        int userPoints = 0;
        Random random = new();

        List<CountryCapital> countryCapitals = CapitalsLoader.GetCountryCapitals();

        string answer;
        do
        {

            QuizSolution quizSolution = PickRandomCountryCapital(countryCapitals, random);
            userPoints += AskQuizQuestion(quizSolution);
            System.Console.WriteLine($"User points : {userPoints}");
            System.Console.WriteLine("""Do you want to quit the program ? Type "y" or "yes" to quit.""");
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

    internal int AskQuizQuestion(QuizSolution quizSolution)
    {
        System.Console.WriteLine($"Here comes the quiz question.");
        int points;
        string userAnswer;
        int answerTryNumber = 1;
        do
        {
            System.Console.WriteLine($"Try nb°{answerTryNumber}. What is the capital of {quizSolution.CountryName} ?");
            System.Console.WriteLine($"Hint : {quizSolution.CapitalName}");
            userAnswer = System.Console.ReadLine();

            if (userAnswer != quizSolution.CapitalName)
            {
                System.Console.WriteLine("Wrong answer.");
                answerTryNumber++;
            };
        } while (userAnswer != quizSolution.CapitalName && answerTryNumber <= 3);

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

        return points;
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
