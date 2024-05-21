# OpenAI Models Comparison Application

## Motivation

I read an article on Medium titled [Let’s compare the OpenAI models in C#](https://medium.com/medialesson/lets-compare-the-openai-models-in-c-916e33e1d539) and found it interesting.
It is written by [Sebastian Jensen](https://medium.com/@tsjdevapps/about).

## Experience

At first I was confused where to get an OpenAI API key from.
I found it on the OpenAI website.

- Go to [API keys](https://platform.openai.com/api-keys) in your account.
- Create a new secret key and use it, when prompted by the application to enter the key.

---

Secondly I got an exception when running the application for the first time.

`Error Code 429 - You exceeded your current quota, please check your plan and billing details.`

I did not know that I need to buy credits to use the OpenAI API.
I thought it would be included in my ChatGPT Plus subscription.

---

Next I wanted to enhance the user experience by adding additional information and commands.

---

## Overview

The OpenAI Models Comparison application is designed to facilitate the comparison of different OpenAI models by allowing users to interact with these models in a unified interface.
The application leverages the Azure AI OpenAI service to access various versions of GPT models and provides a console-based user interface for interaction.
It is not restricted to only use Azure's OpenAI service.

## Features

- **Model Comparison:** Compare responses from different OpenAI models.
- **Temperature Control:** Adjust the temperature setting for model responses to influence the creativity and variability of the outputs.
- **Dynamic Model Selection:** Change the models being compared on the fly.
- **User-Friendly Interface:** Utilize [Spectre.Console](https://spectreconsole.net/) for a rich console interface experience.

## Getting Started

### Prerequisites

- .NET SDK
- OpenAI subscription with access to OpenAI
- OpenAI Project API key
- Credits to use the OpenAI API
- Spectre.Console library

### Installation

1. Clone the repository:

    ```sh
    git clone https://github.com/yourusername/OpenAiModelsComparison.git
    cd OpenAiModelsComparison
    ```

2. Restore the dependencies:

    ```sh
    dotnet restore
    ```

### Configuration

1. Create an OpenAI Project API key on the [OpenAI API keys page](https://platform.openai.com/api-keys).

### Running the Application

1. Build the application:

    ```sh
    dotnet build
    ```

2. Run the application:

    ```sh
    dotnet run
    ```

## Usage

### Main Features

1. **Create Headers:**
    - The application starts by creating a header for the OpenAI Models Comparison.
2. **Login to OpenAI:**
    - The application logs into OpenAI using the provided key through `apiService.LoginOpenAi()`.
3. **Set Temperature:**
    - Prompt to set the temperature for the models’ responses.
4. **Select Models:**
    - Select the models to be compared from available options: `Gpt35TurboKey`, `Gpt4Key`, `Gpt4TurboKey`, `Gpt4OKey`.
5. **Welcome Message:**
    - A welcome message is displayed to the user.
6. **User Interaction:**
    - Users can input their messages to interact with the models.
    - Special commands:
        - `exit`: Exit the application.
        - `temp`: Change the temperature.
        - `models`: Update the model selection.
7. **Model Responses:**
    - The application collects responses from the selected models, ordered by response duration, and displays them.

### Commands

- **exit:** Exits the application gracefully.
- **temp:** Prompts the user to set a new temperature for model responses.
- **models:** Allows the user to update the selected models for comparison.

## Code Structure

- **Namespaces:**
  - `Azure.AI.OpenAI`: For interacting with OpenAI services.
  - `OpenAiModelsComparison.Interfaces`: Interfaces for the application.
  - `OpenAiModelsComparison.Models`: Data models used in the application.
  - `OpenAiModelsComparison.Utils`: Utility functions and helpers.
  - `Spectre.Console`: For console UI enhancements.
- **Classes and Methods:**
  - `Application`: The main class that runs the application logic.
  - `Run()`: The entry point method that initializes and runs the application loop.

## Future Enhancements

- **GUI Integration:** Add a graphical user interface for more user-friendly interaction.
- **Extended Model Support:** Integrate more models by retrieving a list of available models from OpenAI.
- **Logging:** Implement detailed logging for better troubleshooting and analytics.
- **OpenTelemetry Dashboard:** Use .NET Aspire to use their OpenTelemetry dashboard while developing.

## Contributing

Contributions are welcome!
Please fork the repository and submit pull requests for any enhancements or bug fixes.

## License

This project is licensed under the MIT License.
See the LICENSE file for details.

## Contact

For any questions or feedback, please contact [velotist@outlook.com].

## References

- [Let’s compare the OpenAI models in C#](https://medium.com/medialesson/lets-compare-the-openai-models-in-c-916e33e1d539)
- [Sebastian Jensen](https://medium.com/@tsjdevapps/about)
- [OpenAI API keys page](https://platform.openai.com/api-keys)
