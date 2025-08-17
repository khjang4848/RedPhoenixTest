namespace RedPhoenix.Web.Messages;

public class ErrorResponse(string name, string message, string? stackTrace)
{
    public string Name { get; } = name;
    public string Message { get; } = message;
    public string? StackTrace { get; } = stackTrace;
}

