namespace Pipeline
{
    // Represents a log entry to vectorize and index
    public class LogEntry
    {
        public DateTime TimeGenerated { get; set; }
        public string Message { get; set; }
        public int SeverityLevel { get; set; }
        public string OperationName { get; set; }
        public string ExceptionType { get; set; }

        public override string ToString()
        {
            return $"{TimeGenerated:O} [{SeverityLevel}] {OperationName}: {Message} ({ExceptionType})";
        }
    }
}

