using Microsoft.Extensions.Configuration;
using OpenAI.Chat;
using OpenAIIntegration;
using AzureSearchIntegration;
using System.ClientModel;

namespace ChatApp
{
    internal class Chatter
    {
        public async Task Run()
        {
            // Replace with your values
            string userPrompt = "How do I handle quote errors? How frequent they are in the last 2 days?";

            var configuration = new ConfigurationBuilder()
                  .AddUserSecrets<Chatter>()
                  .Build();

            // ===============================
            // 1. Embed the user prompt
            // ===============================

            var embedding = new OpenAIConnector(configuration["OPENAI_API_KEY"]).GetEmbedding(userPrompt);

            // ===============================
            // 2. Query Azure AI Search
            // ===============================

            var searchResults = await new SearchConnector(configuration["Azure_Search_Index_URI"], configuration["Azure_Search_Index_Key"])
                .SearchInAzureAISearch(embedding);

             // ===============================
             // 3. Send results to OpenAI chat
             // ===============================

             var retrievedContext = string.Join("\n---\n", searchResults);

            ChatClient client = new(model: "o3-mini", apiKey: configuration["OPENAI_API_KEY"]);

            AsyncCollectionResult<StreamingChatCompletionUpdate> completionUpdates = client.CompleteChatStreamingAsync(retrievedContext);

            await foreach (StreamingChatCompletionUpdate completionUpdate in completionUpdates)
            {
                if (completionUpdate.ContentUpdate.Count > 0)
                {
                    Console.Write(completionUpdate.ContentUpdate[0].Text);
                }
            }
        }
    }
}
