using System.Diagnostics;
using Azure;
using Azure.AI.OpenAI;
using OpenAiModelsComparison.Interfaces;
using OpenAiModelsComparison.Models;
using OpenAiModelsComparison.Utils;
using Spectre.Console;

namespace OpenAiModelsComparison;

public class ApiService(IConsoleHelper consoleHelper) : IApiService
{
    public async Task<string> LoginOpenAi()
    {
        AnsiConsole.MarkupLine("[bold yellow]Check on https://platform.openai.com/api-keys to get your key.[/]");

        while (true)
        {
            var openAiKey = consoleHelper.GetString("[green]Please insert your [yellow]OpenAI[/] API key: [/]");
            var isApiKeyValid = await ValidateApiKey(openAiKey);
            if (isApiKeyValid) return openAiKey;
        }
    }

    public async Task<ModelResponse> HandleRequest(OpenAIClient client, ChatCompletionsOptions options)
    {
        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine($"[green]{options.DeploymentName}:[/]");

        Stopwatch stopwatch = new();
        stopwatch.Start();

        try
        {
            Response<ChatCompletions> chatCompletionsResponse = await client.GetChatCompletionsAsync(options);

            stopwatch.Stop();

            var messageContent = chatCompletionsResponse.Value.Choices[0].Message.Content;
            AnsiConsole.WriteLine(messageContent);

            options.Messages.Add(new ChatRequestAssistantMessage(messageContent));

            var usageInfo = chatCompletionsResponse.Value.Usage;

            return new ModelResponse(options.DeploymentName,
                usageInfo.PromptTokens,
                usageInfo.CompletionTokens,
                stopwatch.Elapsed);
        }
        catch (RequestFailedException ex)
        {
            stopwatch.Stop();
            AnsiConsole.MarkupLine($"[red]Request failed: {ex.Message}[/]");
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            AnsiConsole.MarkupLine($"[red]An unexpected error occurred: {ex.Message}[/]");
        }

        return new ModelResponse(options.DeploymentName, 0, 0, stopwatch.Elapsed);
    }

    public ChatCompletionsOptions CreateChatCompletionsOptions(string deploymentName, float temperature = 0.5f)
    {
        ChatCompletionsOptions chatCompletionsOptions = new()
        {
            MaxTokens = 1000,
            Temperature = temperature,
            DeploymentName = deploymentName
        };

        chatCompletionsOptions.Messages.Add(new ChatRequestSystemMessage("You are a helpful AI assistant."));

        return chatCompletionsOptions;
    }

    private async Task<bool> ValidateApiKey(string apiKey)
    {
        AnsiConsole.MarkupLine("[bold yellow]Validating API key...[/]");
        try
        {
            OpenAIClient client = new(apiKey);
            var options = new ChatCompletionsOptions
            {
                DeploymentName = GptVersionKeyStore.Gpt35TurboKey,
                Messages =
                {
                    new ChatRequestSystemMessage("Validate API key")
                }
            };

            await client.GetChatCompletionsAsync(options);

            return true;
        }
        catch (RequestFailedException exception)
        {
            HandleRequestFailedException(exception);

            return false;
        }
    }

    private static void HandleRequestFailedException(RequestFailedException exception)
    {
        switch (exception.Status)
        {
            case 401:
                AnsiConsole.MarkupLine("[red]Invalid API key. Please check your key and try again.[/]");
                break;
            case 429:
                AnsiConsole.MarkupLine(
                    "[red]Error: You exceeded your current quota. Please check your plan and billing details.[/]");
                break;
            default:
                AnsiConsole.MarkupLine($"[red]Error: {exception.Message}[/]");
                break;
        }
    }
}
