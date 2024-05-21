using Microsoft.Extensions.DependencyInjection;
using OpenAiModelsComparison.Interfaces;
using OpenAiModelsComparison.Utils;

namespace OpenAiModelsComparison;

public static class Program
{
    public static async Task Main()
    {
        var serviceProvider = ConfigureServices();
        var application = serviceProvider.GetService<Application>();
        if (application != null)
        {
            await application.Run();
        }
    }

    private static ServiceProvider ConfigureServices()
    {
        var services = new ServiceCollection();
        services.AddSingleton<IConsoleHelper, ConsoleHelper>();
        services.AddSingleton<IApiService, ApiService>();
        services.AddSingleton<Application>();

        return services.BuildServiceProvider();
    }
}
