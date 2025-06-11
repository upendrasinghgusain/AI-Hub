using Azure.AI.Projects;
using Azure.Identity;
using Azure.AI.Inference;
using Microsoft.Extensions.Configuration;

namespace HelloWorld
{
    public class Chat1
    {
        public static async Task Run()
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

                Console.WriteLine("Press 'q' to quit or any other key to continue...");
                do
                {
                    Console.WriteLine("Enter a question:");
                    var user_prompt = Console.ReadLine();
                    if (user_prompt == "q" || user_prompt == "Q")
                    {
                        break;
                    }

                    var requestOptions = new ChatCompletionsOptions()
                    {
                        Model = "Ministral-3B",
                        Messages =
                        {
                            new ChatRequestSystemMessage("You are a helpful AI assistant that answers questions."),
                            new ChatRequestUserMessage(user_prompt),
                        }
                    };

                    //Response<ChatCompletions> response = chatClient.Complete(requestOptions);
                    //Console.WriteLine(response.Value.Content);

                    StreamingResponse<StreamingChatCompletionsUpdate> response = await chatClient.CompleteStreamingAsync(requestOptions);
                    await foreach (StreamingChatCompletionsUpdate chatUpdate in response)
                    {
                        if (!string.IsNullOrEmpty(chatUpdate.ContentUpdate))
                        {
                            Console.Write(chatUpdate.ContentUpdate);
                        }
                    }
                    Console.WriteLine();
                    Console.WriteLine();
                } while (true);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
