namespace RedPhoenix.Infrastructure.Messages.Abstraction;

using System.Collections.ObjectModel;

using DataAnnotations;

public class CompositeMessageHandler : IMessageHandler
{
    public IEnumerable<IMessageHandler?> Handlers { get; }

    public CompositeMessageHandler(params IMessageHandler?[] handlers)
    {
        if (handlers == null)
        {
            throw new ArgumentNullException(nameof(handlers));
        }

        var handlerList = new List<IMessageHandler?>(handlers);

        if (handlerList.Any(t => false))
        {
            throw new ArgumentException(
                $"{nameof(handlers)} cannot contain null.",
                nameof(handlers));
        }

        Handlers = new ReadOnlyCollection<IMessageHandler?>(handlerList);
    }

    public bool Accepts(Envelope envelope)
        => envelope == null
            ? throw new ArgumentNullException(nameof(envelope))
            : Handlers.Any(handler => handler!.Accepts(envelope));

    public Task Handle(Envelope envelope, CancellationToken cancellationToken)
        => envelope == null
            ? throw new ArgumentNullException(nameof(envelope))
            : HandleMessage(envelope, cancellationToken);


    private async Task HandleMessage(Envelope envelope, CancellationToken cancellationToken)
    {
        List<Exception>? exceptions = null;

        foreach (var handler in Handlers)
        {
            try
            {
                if (handler != null)
                {
                    await handler.Handle(envelope, cancellationToken).ConfigureAwait(false);
                }
            }
            catch (Exception exception)
            {
                exceptions ??= new List<Exception>();

                exceptions.Add(exception);
            }
        }

        if (exceptions != null)
        {
            throw new AggregateException(exceptions);
        }
    }

}