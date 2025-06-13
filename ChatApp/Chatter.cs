using Microsoft.Extensions.Configuration;
using OpenAI.Chat;
using System.ClientModel;

namespace ChatApp
{
    internal class Chatter
    {
        public async Task Run()
        {
            var configuration = new ConfigurationBuilder()
                   .AddUserSecrets<Chatter>()
                   .Build();

            ChatClient client = new(model: "o3-mini", apiKey: configuration["OPENAI_API_KEY"]);

            Console.WriteLine("Press 'q' to quit or any other key to continue...");
            do
            {
                Console.WriteLine("Enter a question:");
                var userPrompt = Console.ReadLine();
                if (userPrompt == "q" || userPrompt == "Q")
                {
                    break;
                }

                AsyncCollectionResult<StreamingChatCompletionUpdate> completionUpdates = client.CompleteChatStreamingAsync(userPrompt);

                await foreach (StreamingChatCompletionUpdate completionUpdate in completionUpdates)
                {
                    if (completionUpdate.ContentUpdate.Count > 0)
                    {
                        Console.Write(completionUpdate.ContentUpdate[0].Text);
                    }
                }

                Console.WriteLine();
                Console.WriteLine();

            } while (true);
        }
    }
}
