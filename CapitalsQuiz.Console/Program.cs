using CapitalsQuiz.Console;
using CapitalsQuiz.Console.DB;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

builder.Configuration.SetBasePath(AppContext.BaseDirectory).AddJsonFile("appsettings.json");

builder.Logging.AddConfiguration(builder.Configuration.GetSection("Logging")).AddConsole();

builder.Services.AddDbContext<QuizContext>(options => options.UseSqlite(GetConnectionString()));

builder.Services.AddHostedService<QuizHostedService>();

using IHost host = builder.Build();

await host.RunAsync();

static string GetConnectionString()
{
    string projectPath = Path.GetFullPath(@"..\..\..");
    string dbPath = Path.Combine(projectPath, "DB", "quiz.db");
    return $"Data Source={dbPath}";
}