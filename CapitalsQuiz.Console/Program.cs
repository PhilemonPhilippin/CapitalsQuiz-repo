using CapitalsQuiz.Console.DB;
using CapitalsQuiz.Console.Quiz;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

builder.Services.AddDbContext<QuizContext>(options => options.UseSqlite(GetConnectionString()));
using IHost host = builder.Build();

CancellationTokenSource cts = new();
EnsureDatabaseReady(host.Services);
await RunQuizAsync(host.Services, cts);

try
{
    await host.RunAsync(cts.Token);
}
catch (OperationCanceledException)
{
    Console.WriteLine("Application is shutting down...");
}

static void EnsureDatabaseReady(IServiceProvider hostProvider)
{
    using IServiceScope serviceScope = hostProvider.CreateScope();

    QuizContext dbContext = serviceScope.ServiceProvider.GetRequiredService<QuizContext>();
    dbContext.Database.EnsureCreated();
}

static async Task RunQuizAsync(IServiceProvider services, CancellationTokenSource cts)
{
    using IServiceScope serviceScope = services.CreateScope();
    QuizContext context = serviceScope.ServiceProvider.GetRequiredService<QuizContext>();
    Quiz quiz = new(context);
    await quiz.Run();
    cts.Cancel();
}

static string GetConnectionString()
{
    string projectPath = Path.GetFullPath(@"..\..\..");
    string dbPath = Path.Combine(projectPath, "DB", "quiz.db");
    string connectionString = $"Data Source={dbPath}";
    return connectionString;
}