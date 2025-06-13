using OpenAI.Embeddings;

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
    }
}
