using Azure.AI.OpenAI;
using OpenAiModelsComparison.Interfaces;
using OpenAiModelsComparison.Models;
using OpenAiModelsComparison.Utils;
using Spectre.Console;

namespace OpenAiModelsComparison;

public class Application(IConsoleHelper consoleHelper, IApiService apiService)
{
    public async Task Run()
    {
        consoleHelper.CreateHeader("OpenAI Models Comparison");
        var openAiKey = await apiService.LoginOpenAi();

        consoleHelper.CreateHeader("OpenAI Models Comparison");
        consoleHelper.CreateTemperatureHeader();
        var temperature = consoleHelper.GetTemperature();

        consoleHelper.CreateHeader("OpenAI Models Comparison");
        var selectedModels = consoleHelper.GetSelection(new[]
        {
            GptVersionKeyStore.Gpt35TurboKey,
            GptVersionKeyStore.Gpt4Key,
            GptVersionKeyStore.Gpt4TurboKey,
            GptVersionKeyStore.Gpt4OKey
        });

        consoleHelper.CreateHeader("OpenAI Models Comparison");
        consoleHelper.CreateChatWelcome();

        OpenAIClient client = new(openAiKey);

        var chatCompletionsOptions = selectedModels
            .Select(model => apiService.CreateChatCompletionsOptions(model, temperature))
            .ToList();

        while (true)
        {
            var userMessage = consoleHelper.GetString("[green]USER:[/]");

            if (userMessage.Equals("exit", StringComparison.CurrentCultureIgnoreCase))
            {
                AnsiConsole.MarkupLine("[red]Exiting the application...[/]");
                break;
            }

            if (userMessage.Equals("temp", StringComparison.CurrentCultureIgnoreCase))
            {
                temperature = consoleHelper.GetTemperature();
                chatCompletionsOptions.ForEach(options => options.Temperature = temperature);
                AnsiConsole.MarkupLine($"[yellow]Temperature set to {temperature}[/]");
                continue;
            }

            if (userMessage.Equals("models", StringComparison.CurrentCultureIgnoreCase))
            {
                selectedModels = consoleHelper.GetSelection(new[]
                {
                    GptVersionKeyStore.Gpt35TurboKey,
                    GptVersionKeyStore.Gpt4Key,
                    GptVersionKeyStore.Gpt4TurboKey,
                    GptVersionKeyStore.Gpt4OKey
                });
                chatCompletionsOptions = selectedModels
                    .Select(model => apiService.CreateChatCompletionsOptions(model, temperature))
                    .ToList();
                AnsiConsole.MarkupLine("[yellow]Model selection updated[/]");

                continue;
            }

            var modelResponses = new List<ModelResponse>();
            foreach (var options in chatCompletionsOptions.Where(_ => !string.IsNullOrEmpty(userMessage)))
            {
                options.Messages.Add(new ChatRequestUserMessage(userMessage));

                var response = await apiService.HandleRequest(client, options);
                modelResponses.Add(response);
            }

            var orderedByDuration = modelResponses
                .OrderBy(modelResponse => modelResponse.Duration)
                .ToList();

            AnsiConsole.WriteLine();
            AnsiConsole.WriteLine();
            consoleHelper.CreateHeader("Results", false);
            consoleHelper.CreateOutputInfo(orderedByDuration);
        }
    }
}
