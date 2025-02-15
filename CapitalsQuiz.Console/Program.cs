using CapitalsQuiz.Console.DB;
using CapitalsQuiz.Console.Quiz;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

builder.Services.AddDbContext<QuizContext>(options => options.UseSqlite(GetConnectionString()));
using IHost host = builder.Build();

ScopeTheHost(host.Services);
await host.RunAsync();

Quiz quiz = new();
quiz.Run();


static void ScopeTheHost(IServiceProvider hostProvider)
{
    using IServiceScope serviceScope = hostProvider.CreateScope();
    QuizContext dbContext = serviceScope.ServiceProvider.GetRequiredService<QuizContext>();
    dbContext.Database.EnsureCreated();
}

static string GetConnectionString()
{
    // Three levels up from "\bin\Debug\net8.0".
    string projectPath = Path.GetFullPath(@"..\..\..");
    // Path to my quiz.db.
    string dbPath = Path.Combine(projectPath, "DB", "quiz.db");
    string connectionString = $"Data Source={dbPath}";

    return connectionString;
}