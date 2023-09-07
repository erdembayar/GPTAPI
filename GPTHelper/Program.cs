using System.Numerics;
using System.Text;
using System;
using Newtonsoft.Json.Linq;
using CodeHelper;

internal class Program
{
    private static readonly HttpClient HttpClient = new HttpClient();

    public enum Mode
    {
        Marketer,
        CodeHelper
    }

    static async Task Main(string[] args)
    {
        Mode mode = Mode.Marketer;
        string systemContent = mode == Mode.Marketer ? "Customer is using AI chatbot and you're their AI marketing assistant. " +
            "You need to generate marketing ideas for promoting their product"
            
            :

@"A software developer is using an AI chatbot in a code editor and you are their AI programming assistant. When asked for your name, you must respond with ""Ask ChatGPT"".
•	Follow the user's requirements carefully & to the letter
•	Minimize any other prose
•	Your responses should be informative and logical. You should always adhere to technical information
•	If the user asks for code or technical questions, you must provide code suggestions and adhere to technical information
•	You should always generate short suggestions for the next user turns that are relevant to the conversation and not offensive
•	You also have a background as a highly skilled professor specializing in software development. If the programming language is not clear from the context or question, you will answer in the context of C#.
•	If the question is related to a developer, you must respond with content related to a developer.
•	Provide well thought out examples
•	Utilize provided context in examples
•	Match the style of provided context when using examples
•	When generating code ensure it's readable and indented properly
•	When using code in examples write out the code in markdown. Make sure to include the programming language name at the start of the Markdown code blocks
•	When explaining code, add a final paragraph describing possible ways to improve the code with respect to readability and performance
•	List two additional prompts that would be good follow up questions to your explanation. FollowUpPrompts should be directed at you and listed on the same line, separated by a |
I hope that helps! Let me know if you have any further questions related to development.
";

        var conversation = new JArray
        {
            new JObject
            {
                ["role"] = "system",
                ["content"] = systemContent
            }
        };

        //string userInputPrompt = "User input:";
        string userInputPrompt = mode == Mode.Marketer ?
            "Hello! Let's build together. \r\nI'm your AI powered marketing helper, so surprises and mistakes are possible. Make sure to \r\nverify any AI code or suggestions, and share feedback so that we can learn and improve:\r\n>>"
            :
            "Hello! Let's build together. \r\nI'm your AI powered coder helper, so surprises and mistakes are possible. Make sure to \r\nverify any AI code or suggestions, and share feedback so that we can learn and improve:\r\n>>"
            ;

        string userName = mode == Mode.Marketer ? "Marketer" : "Developer";

        while (true)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;  // Set the text color to yellow
            Console.WriteLine(userInputPrompt);
            Console.ResetColor();  // Reset the text color to its default value

            var user_input = Console.ReadLine();

            conversation.Add(new JObject
            {
                ["role"] = "user",
                ["content"] = user_input,
                ["name"] = userName
            });

            var response = await ChatCompletion(conversation);

            conversation.Add(new JObject
            {
                ["role"] = "assistant",
                ["content"] = response["choices"][0]["message"]["content"].ToString()
            });

            Console.WriteLine("\n" + response["choices"][0]["message"]["content"] + "\n");
        }
    }

    static async Task<JObject> ChatCompletion(JArray messages, float temperature = 0)
    {
        string apiUrl = "https://api.openai.com/v1/chat/completions";
        string apiKey = ContextProvider.APIKey;
        HttpClient.DefaultRequestHeaders.Clear();
        HttpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

        var request = new JObject
        {
            ["model"] = "gpt-3.5-turbo",
            ["temperature"] = temperature,
            ["messages"] = messages
        };

        var response = await HttpClient.PostAsync(
            apiUrl,
            new StringContent(request.ToString(), Encoding.UTF8, "application/json"));

        var responseBody = await response.Content.ReadAsStringAsync();
        return JObject.Parse(responseBody);
    }
}