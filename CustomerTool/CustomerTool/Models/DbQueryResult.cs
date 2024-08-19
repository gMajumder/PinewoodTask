namespace CustomerTool.Models
{
    public class DbQueryResult<T>
    {
        public bool IsQuerySuccessful { get; set; }
        public T QueryResult { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
        public Exception? Exception { get; set; }
    }
}
