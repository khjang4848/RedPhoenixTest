namespace RedPhoenix.Test;

public class HttpMessageOptions
{
    private const string AnyValue = "*";
    private HttpContent? _httpContent;
    private string? _httpContentSerialized;
    private int _numberOfTimesCalled = 0;

    public Uri? RequestUri { get; set; }

    public HttpMethod? HttpMethod { get; set; }

    public HttpResponseMessage? HttpResponseMessage { get; set; }

    public HttpContent? HttpContent
    {
        get => _httpContent;
        set
        {
            _httpContent = value;
            _httpContentSerialized = Task.Run(async () => await _httpContent!.ReadAsStringAsync()).Result;
        }
    }

    public IDictionary<string, IEnumerable<string>>? Headers { get; set; }

    public int NumberOfTimesCalled => _numberOfTimesCalled;

    public void IncrementNumberOfTimesCalled()
        => Interlocked.Increment(ref _numberOfTimesCalled);

    public override string ToString()
    {
        var httpMethodText = HttpMethod?.ToString() ?? AnyValue;

        var headers = Headers!.Any() ?
            " || Headers: " + string.Join(":",
                Headers!.Select(x
                    => $"{x.Key}|{string.Join(",", x.Value)}"))
            : "";

        return $"{httpMethodText} {RequestUri}{(HttpContent != null ? $" || body/content: {_httpContentSerialized}" : "")}{headers}";
    }
}

