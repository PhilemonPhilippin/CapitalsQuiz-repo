using CapitalsQuiz.Console.DB;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CapitalsQuiz.Console;

internal class QuizHostedService(IServiceProvider services, IHostApplicationLifetime hostApplicationLifeTime) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        EnsureDatabaseReady();
        await RunQuizAsync();
        hostApplicationLifeTime.StopApplication();
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        System.Console.WriteLine("Application is shutting down...");
        return Task.CompletedTask;
    }

    private void EnsureDatabaseReady()
    {
        using IServiceScope serviceScope = services.CreateScope();
        QuizContext dbContext = serviceScope.ServiceProvider.GetRequiredService<QuizContext>();
        dbContext.Database.EnsureCreated();
    }

    private async Task RunQuizAsync()
    {
        using IServiceScope serviceScope = services.CreateScope();
        QuizContext context = serviceScope.ServiceProvider.GetRequiredService<QuizContext>();
        Quiz.Quiz quiz = new(context);
        await quiz.Run();
    }
}