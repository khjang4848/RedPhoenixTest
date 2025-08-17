namespace RedPhoenix.Test;

using System.Net;

public class HttpMessageHandlerDouble : HttpMessageHandler
{
    private readonly List<CannedAnswer> _cannedAnswers = [];

    public void AddAnswer(
        Func<HttpRequestMessage, bool> predicate,
        HttpResponseMessage answer)
        => _cannedAnswers.Add(new CannedAnswer(predicate, answer));

    public void AddAnswer(
        Func<HttpRequestMessage, bool> predicate,
        HttpStatusCode statusCode)
        => AddAnswer(predicate, new HttpResponseMessage(statusCode));

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        await Task.Delay(millisecondsDelay: 1, cancellationToken);

        foreach (var cannedAnswer in
                 _cannedAnswers.Where(x => x.Predicate.Invoke(request)))
        {
            return cannedAnswer.Answer;
        }

        return new HttpResponseMessage(HttpStatusCode.NotImplemented);
    }
}

public record CannedAnswer(
    Func<HttpRequestMessage, bool> Predicate,
    HttpResponseMessage Answer)
{
    public Func<HttpRequestMessage, bool> Predicate { get; } = Predicate;

    public HttpResponseMessage Answer { get; } = Answer;
}

