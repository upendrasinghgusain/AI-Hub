using System;
using Azure;
using Azure.AI.Projects;
using Azure.Identity;
using Azure.AI.Inference;
using Microsoft.Extensions.Configuration;

namespace HelloWorld
{
    class Chat1
    {
        public static void Run()
        {
            try
            {
                var config = new ConfigurationBuilder()
                    .AddUserSecrets<Chat1>()
                    .Build();
                var projectEndpoint = config["Azure_AI_Foundry_project_endpoint"];

                // Initialize the project client
                var projectClient = new AIProjectClient(
                    new Uri(projectEndpoint!),
                    new DefaultAzureCredential());

                // Get a chat client
                ChatCompletionsClient chatClient = projectClient.GetChatCompletionsClient();

                // Get a chat completion based on a user-provided prompt
                Console.WriteLine("Enter a question:");
                var user_prompt = Console.ReadLine();

                var requestOptions = new ChatCompletionsOptions()
                {
                    Model = "Ministral-3B",
                    Messages =
                        {
                            new ChatRequestSystemMessage("You are a helpful AI assistant that answers questions."),
                            new ChatRequestUserMessage(user_prompt),
                        }
                };

                Response<ChatCompletions> response = chatClient.Complete(requestOptions);
                Console.WriteLine(response.Value.Content);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
