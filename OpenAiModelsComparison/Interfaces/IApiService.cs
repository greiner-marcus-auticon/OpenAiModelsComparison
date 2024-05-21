using Azure.AI.OpenAI;
using OpenAiModelsComparison.Models;

namespace OpenAiModelsComparison.Interfaces;

public interface IApiService
{
    Task<string> LoginOpenAi();
    ChatCompletionsOptions CreateChatCompletionsOptions(string deploymentName, float temperature = 0.5f);
    Task<ModelResponse> HandleRequest(OpenAIClient client, ChatCompletionsOptions options);
}
