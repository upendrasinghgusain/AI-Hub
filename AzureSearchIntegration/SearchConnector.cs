using Azure;
using Azure.Search.Documents;
using Azure.Search.Documents.Models;

namespace AzureSearchIntegration
{
    public class SearchConnector
    {
        private readonly string _searchIndexUri;
        private readonly string _searchIndexKey;
        private readonly SearchClient _searchClient;

        public SearchConnector(string searchIndexUri, string searchIndexKey)
        {
            _searchIndexUri = searchIndexUri;
            _searchIndexKey = searchIndexKey;

            _searchClient = new SearchClient(
                new Uri(_searchIndexUri),
                "my-vector-index",
                new AzureKeyCredential(_searchIndexKey)
            );
        }

        public async Task UploadToAzureAISearch(List<LogSearchEntry> logSearchEntries)
        {
            List<dynamic> documents = new List<dynamic>();
            foreach (var logSearchEntry in logSearchEntries)
            {
                documents.Add(new
                {
                    id = Guid.NewGuid().ToString(),
                    content = logSearchEntry.Content,
                    contentVector = logSearchEntry.Embedding
                });
            }
            

            await _searchClient.UploadDocumentsAsync(documents);
        }

        public async Task<List<string>> SearchInAzureAISearch(float[] embedding)
        {
            SearchResults<LogEntry> response = await _searchClient.SearchAsync<LogEntry>(
                new SearchOptions
                {
                    VectorSearch = new()
                    {
                        Queries = { new VectorizedQuery(embedding) { KNearestNeighborsCount = 3, Fields = { "contentVector" } } }
                    }
                });

            List<string> results = new List<string>();

            await foreach (SearchResult<LogEntry> result in response.GetResultsAsync())
            {
                results.Add(result.Document.content);
            }

            return results;
        }
    }
}
