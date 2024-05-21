namespace OpenAiModelsComparison.Models;

public record ModelResponse(
    string DeploymentName,
    int PromptTokens,
    int CompletionTokens,
    TimeSpan Duration);