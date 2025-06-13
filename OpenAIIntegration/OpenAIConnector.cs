using OpenAI.Chat;
using OpenAI.Embeddings;
using System.ClientModel;

namespace OpenAIIntegration
{
    public class OpenAIConnector
    {
        private readonly string _apiKey;

        public OpenAIConnector(string apiKey)
        {
            _apiKey = apiKey;
        }

        public float[] GetEmbedding(string text)
        {
            EmbeddingClient client = new("text-embedding-3-small", _apiKey);
            OpenAIEmbedding embedding = client.GenerateEmbedding(text);

            return embedding.ToFloats().ToArray();
        }

        public async Task GetChtCompletion(string context, string userPrompt)
        {
            ChatClient client = new(model: "gpt-4o", apiKey: _apiKey);

            List<ChatMessage> messages =
            [
                new SystemChatMessage("You are an AI assistant helping diagnose production issues using logs."),
                new UserChatMessage($"Here are some log entries:\n{context}\n\nBased on these logs, {userPrompt}"),
            ];


            AsyncCollectionResult<StreamingChatCompletionUpdate> completionUpdates = client.CompleteChatStreamingAsync(messages);

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
