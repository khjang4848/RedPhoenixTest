namespace RedPhoenix.Test;

using System.Collections.Concurrent;

using RedPhoenix.Infrastructure.Messages.Abstraction;
using RedPhoenix.Infrastructure.Messages.DataAnnotations;

public class InProcessMessageLogger : IMessageBus
{
    private readonly ConcurrentQueue<Envelope> _log = new();

    public IEnumerable<Envelope> Log => _log;

    public async Task Send(Envelope envelope, CancellationToken cancellationToken)
    {
        await Task.Delay(millisecondsDelay: 1, cancellationToken: cancellationToken);
        _log.Enqueue(envelope);
    }

    public async Task Send(IEnumerable<Envelope> envelopes,
        CancellationToken cancellationToken)
    {
        await Task.Delay(millisecondsDelay: 1, cancellationToken: cancellationToken);
        foreach (var envelope in envelopes)
        {
            _log.Enqueue(envelope);
        }
    }

    public void ClearLog() => _log.Clear();
}
