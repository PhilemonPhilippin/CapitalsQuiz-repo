using CapitalsQuiz.Console.Entities;
using System.Text.Json;


namespace CapitalsQuiz.Console.Quiz;

internal class CapitalsLoader
{
    internal List<CountryCapital> GetCountryCapitals()
    {
        // Three levels up from "\bin\Debug\net8.0".
        string projectPath = Path.GetFullPath(@"..\..\..");

        // Full path to my country-capitals.json
        string capitalsPath = Path.Combine(projectPath, "assets", "country-capitals.json");

        string text = File.ReadAllText(capitalsPath);
        return JsonSerializer.Deserialize<List<CountryCapital>>(text);
    }
}
