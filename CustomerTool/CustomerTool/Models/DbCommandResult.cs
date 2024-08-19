namespace CustomerTool.Models
{
    public class DbCommandResult<T>
    {
        public bool IsCommandSuccessful { get; set; }
        public T Data { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
        public Exception? Exception { get; set; }
        public DbCommandError Error { get; set; } = DbCommandError.None;
    }

    public class DbCommandResult
    {
        public bool IsCommandSuccessful { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
        public Exception? Exception { get; set; }
        public DbCommandError Error { get; set; } = DbCommandError.None;
    }

    public enum DbCommandError
    { 
        RecordNotFound,
        InvalidRequest,
        Other,
        None
    }
}
