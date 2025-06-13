using Azure;
using Azure.Search.Documents;
using Azure.Search.Documents.Indexes.Models;
using System;

namespace Pipeline
{
    internal class AzureAISearchHandler
    {
        private readonly string _searchIndexUri;
        private readonly string _searchIndexKey;

        public AzureAISearchHandler(string searchIndexUri, string searchIndexKey)
        {
            _searchIndexUri = searchIndexUri;
            _searchIndexKey = searchIndexKey;
        }
        public async Task UploadToAzureAISearch(LogEntry logEntries, float[] embedding)
        {
            var searchClient = new SearchClient(
                new Uri(_searchIndexUri),
                "my-vector-index",
                new AzureKeyCredential(_searchIndexKey)
            );

            var document = new
            {
                id = Guid.NewGuid().ToString(),
                content = logEntries.ToString(),
                contentVector = embedding
            };

            await searchClient.UploadDocumentsAsync(new[] { document });
        }
    }
}
