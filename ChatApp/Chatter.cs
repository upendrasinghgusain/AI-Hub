using Microsoft.Extensions.Configuration;
using OpenAIIntegration;
using AzureSearchIntegration;

namespace ChatApp
{
    internal class Chatter
    {
        public async Task Run()
        {
            Console.WriteLine("Welcome to the AI Chat App! Please enter your question or prompt below:");
            string userPrompt = Console.ReadLine();

            var configuration = new ConfigurationBuilder()
                  .AddUserSecrets<Chatter>()
                  .Build();

            // ===============================
            // 1. Embed the user prompt
            // ===============================
            var openAIConnector = new OpenAIConnector(configuration["OPENAI_API_KEY"]);
            var embedding = openAIConnector.GetEmbedding(userPrompt);

            // ===============================
            // 2. Query Azure AI Search
            // ===============================

            var searchResults = await new SearchConnector(configuration["Azure_Search_Index_URI"], configuration["Azure_Search_Index_Key"])
                .SearchInAzureAISearch(embedding);

            // ===============================
            // 3. Send results to OpenAI chat
            // ===============================

            var retrievedContext = string.Join("\n---\n", searchResults);
            await openAIConnector.GetChtCompletion(retrievedContext, userPrompt);

            Console.WriteLine("\n\n\n");
        }
    }
}
