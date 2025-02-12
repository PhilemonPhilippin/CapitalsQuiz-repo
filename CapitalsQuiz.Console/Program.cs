using CapitalsQuiz.Console.Entities;
using CapitalsQuiz.Console.Quiz;

CapitalsLoader loader = new();
List<CountryCapital> list = loader.GetCountryCapitals();

Console.WriteLine(list[0].CountryName);
Console.WriteLine(list[0].CountryCode);
Console.WriteLine(list[0].CapitalName);
Console.WriteLine(list[0].CapitalLatitude);
Console.WriteLine(list[0].CapitalLongitude);
Console.WriteLine(list[0].ContinentName);
Console.WriteLine(list.Count);