using OpenAiModelsComparison.Models;

namespace OpenAiModelsComparison.Interfaces;

public interface IConsoleHelper
{
    void CreateHeader(string text, bool clearConsole = true);
    void CreateChatWelcome();
    string GetString(string prompt);
    IEnumerable<string> GetSelection(IEnumerable<string> options);
    void CreateTemperatureHeader();
    float GetTemperature();
    void CreateOutputInfo(IEnumerable<ModelResponse> modelResponses);
}
