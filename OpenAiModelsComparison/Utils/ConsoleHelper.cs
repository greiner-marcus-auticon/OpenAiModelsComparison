using System.Globalization;
using OpenAiModelsComparison.Interfaces;
using OpenAiModelsComparison.Models;
using Spectre.Console;

namespace OpenAiModelsComparison.Utils;

public class ConsoleHelper : IConsoleHelper
{
    public void CreateChatWelcome()
    {
        AnsiConsole.MarkupLine("[bold yellow]Welcome to the OpenAI Models Comparison Chat![/]");
        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine("[bold yellow]Type 'exit' to exit the application[/]");
        AnsiConsole.MarkupLine("[bold blue]Type 'temp' to change the temperature[/]");
        AnsiConsole.MarkupLine("[bold violet]Type 'models' to change the models[/]");
        AnsiConsole.WriteLine();
    }

    public string GetString(string prompt)
    {
        return AnsiConsole.Ask<string>(prompt);
    }

    public IEnumerable<string> GetSelection(
        IEnumerable<string> options)
    {
        var models = AnsiConsole.Prompt(
            new MultiSelectionPrompt<string>()
                .Title("[green]Please select from the options[/].")
                .Required()
                .PageSize(10)
                .InstructionsText(
                    "[grey](Press [yellow]<space>[/] to toggle your selection and [yellow]<enter>[/] to accept)[/]")
                .AddChoices(options));

        return models;
    }

    public void CreateTemperatureHeader()
    {
        AnsiConsole.MarkupLine("[bold yellow]Please enter the temperature for the models.[/]");
        AnsiConsole.MarkupLine("[bold yellow]The temperature is a value between 0 and 1.[/]");
        AnsiConsole.MarkupLine("[bold yellow]The higher the temperature, the more creative the response.[/]");
        AnsiConsole.WriteLine();
    }

    public float GetTemperature()
    {
        var currentCulture = CultureInfo.CurrentCulture;
        var decimalSeparator = currentCulture.NumberFormat.NumberDecimalSeparator;
        const float defaultTemperature = 0.5f;

        while (true)
        {
            var input = AnsiConsole.Prompt(
                new TextPrompt<string>(
                        $"[green]Enter the temperature (default is {defaultTemperature.ToString(currentCulture)}):[/]")
                    .AllowEmpty());

            if (string.IsNullOrWhiteSpace(input)) return defaultTemperature;

            if (TryParseTemperature(input, currentCulture, out var temperature)) return temperature;

            AnsiConsole.MarkupLine(
                $"[red]Invalid input. Please enter a valid number between 0 and 1 using '{decimalSeparator}' as the decimal separator.[/]");
            AnsiConsole.MarkupLine(
                $"[yellow]Examples of valid inputs: 0{decimalSeparator}5 or {decimalSeparator}5 or +0{decimalSeparator}5 or -0{decimalSeparator}5 or 0 or 1 or 0{decimalSeparator}5.[/]");
        }
    }

    public void CreateOutputInfo(IEnumerable<ModelResponse> modelResponses)
    {
        var table = new Table();
        table.Border(TableBorder.Ascii);
        table.Expand();
        CreateTableHeader(table);

        foreach (var response in modelResponses)
            table.AddRow(
                response.DeploymentName,
                response.PromptTokens.ToString(),
                response.CompletionTokens.ToString(),
                response.Duration.ToString());

        AnsiConsole.Write(table);
        AnsiConsole.WriteLine();
    }

    public void CreateHeader(string text, bool clearConsole = true)
    {
        if (clearConsole)
        {
            AnsiConsole.Clear();
        }

        Grid grid = new();
        grid.AddColumn();
        grid.AddRow(new FigletText(text)
            .Centered()
            .Color(Color.Red));
        AnsiConsole.Write(grid);
        AnsiConsole.WriteLine();
    }

    private static bool TryParseTemperature(string input, IFormatProvider formatProvider, out float temperature)
    {
        var isValidFormat = float.TryParse(input, NumberStyles.Float, formatProvider, out temperature);
        if (!isValidFormat) return false;

        var isInRange = temperature is >= 0 and <= 1;

        return isInRange;
    }

    private static void CreateTableHeader(Table table)
    {
        table.AddColumn("Model");
        table.AddColumn("Prompt Tokens");
        table.AddColumn("Completion Tokens");
        table.AddColumn("Elapsed Time");
    }
}
