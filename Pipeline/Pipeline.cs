using Microsoft.Extensions.Configuration;
using OpenAIIntegration;

namespace Pipeline
{
    public class Pipeline
    {
        private readonly IConfigurationRoot _configuration;

        public Pipeline()
        {
            _configuration = new ConfigurationBuilder()
                   .AddUserSecrets<Program>()
                   .Build();
        }

        public void Run()
        {
            var logs = new LogAnalyticsHandler(_configuration["Log_Analytics_Workspace_Id"]).RunQuery();

            var testLog = "2025-06-12T14:22:54.3473314Z [3] POST Quotes/CreateQuote: [{\"severityLevel\":\"Error\",\"outerId\":\"0\",\"message\":\"Simulated quote failure due to invalid customer name.\",\"type\":\"System.InvalidOperationException\",\"id\":\"6124118\",\"parsedStack\":[{\"assembly\":\"Quoting, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\",\"method\":\"QuotesController.CreateQuote\",\"level\":0,\"line\":24,\"fileName\":\"C:\\\\Development\\\\AI-Hub\\\\Quoting\\\\Controllers\\\\QuotesController.cs\"}]}] (AppExceptions)";

            var embedding = new OpenAIConnector(_configuration["OPENAI_API_KEY"]).GetEmbedding(testLog);

            //// STEP 3: Upload to Azure AI Search
        }

    }

}

