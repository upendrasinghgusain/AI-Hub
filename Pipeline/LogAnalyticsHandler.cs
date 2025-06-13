using Azure;
using Azure.Identity;
using Azure.Monitor.Query;
using Azure.Monitor.Query.Models;

namespace Pipeline
{
    internal class LogAnalyticsHandler
    {
        private readonly string _workspaceId;

        public LogAnalyticsHandler(string workspaceId)
        {
            _workspaceId = workspaceId;
        }

        public List<LogEntry> RunQuery()
        {
            var client = new LogsQueryClient(new DefaultAzureCredential());

            string query = @"
                AppExceptions
                | top 10 by TimeGenerated";

            Response<LogsQueryResult> logsQueryResponse = client.QueryWorkspace(_workspaceId, query, QueryTimeRange.All);

            List<LogEntry> logEntries = new List<LogEntry>();

            var table = logsQueryResponse.Value.Table;

            foreach (var row in table.Rows)
            {
                try
                {
                    var logEntry = new LogEntry
                    {
                        TimeGenerated = row.GetDateTimeOffset("TimeGenerated")?.UtcDateTime ?? DateTime.MinValue,
                        Message = row.GetString("Details") ?? string.Empty,
                        SeverityLevel = row.GetInt32("SeverityLevel") ?? 0,
                        OperationName = row.GetString("OperationName") ?? string.Empty,
                        ExceptionType = row.GetString("Type") ?? string.Empty
                    };

                    logEntries.Add(logEntry);
                    //Console.WriteLine($"Log Entry: {logEntry.TimeGenerated} - {logEntry.Message} - {logEntry.SeverityLevel} - {logEntry.OperationName} - {logEntry.ExceptionType}");
                }
                catch (Exception ex)
                {

                    throw;
                }

            }

            return logEntries;
        }
    }
}
