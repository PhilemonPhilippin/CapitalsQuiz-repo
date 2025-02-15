using CapitalsQuiz.Console.DB;
using CapitalsQuiz.Console.Quiz;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

builder.Services.AddDbContext<QuizContext>(options => options.UseSqlite(GetConnectionString()));
using IHost host = builder.Build();

CancellationTokenSource cts = new();
EnsureDatabaseReady(host.Services, cts.Token);

await RunQuizAsync(cts);

try
{
    await host.RunAsync(cts.Token);
}
catch (OperationCanceledException)
{
    Console.WriteLine("Application is shutting down...");
}

static void EnsureDatabaseReady(IServiceProvider hostProvider, CancellationToken cancellationToken)
{
    using IServiceScope serviceScope = hostProvider.CreateScope();

    QuizContext dbContext = serviceScope.ServiceProvider.GetRequiredService<QuizContext>();
    dbContext.Database.EnsureCreated();
}

static async Task RunQuizAsync(CancellationTokenSource cts)
{
    Quiz quiz = new();
    await quiz.Run();
    cts.Cancel();
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