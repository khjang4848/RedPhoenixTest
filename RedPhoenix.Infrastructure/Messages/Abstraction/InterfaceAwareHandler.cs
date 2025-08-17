namespace RedPhoenix.Infrastructure.Messages.Abstraction;

using System.Collections.ObjectModel;
using System.Reflection;

using DataAnnotations;

public abstract class InterfaceAwareHandler : IMessageHandler
{
    private readonly IReadOnlyDictionary<Type, Handler> _handlers;

    protected InterfaceAwareHandler()
    {
        var handlers = new Dictionary<Type, Handler>();

        WireUpHandler(handlers);

        _handlers = new ReadOnlyDictionary<Type, Handler>(handlers);
    }

    public virtual bool Accepts(Envelope envelope)
        => envelope == null
            ? throw new ArgumentNullException(nameof(envelope))
            : _handlers.ContainsKey(envelope.Message?.GetType()!);

    public Task Handle(Envelope envelope, CancellationToken cancellationToken)
        => envelope == null
            ? throw new ArgumentNullException(nameof(envelope))
            : _handlers.TryGetValue(envelope.Message?.GetType()!, out var handler)
                ? handler.Invoke(envelope, cancellationToken)
                : Task.CompletedTask;

    private void WireUpHandler(Dictionary<Type, Handler> handlers)
    {
        var factoryTemplate = typeof(InterfaceAwareHandler)
        .GetTypeInfo().GetDeclaredMethod(nameof(GetMessageHandler));

        var query = from t in GetType().GetTypeInfo().ImplementedInterfaces
                    where t.IsConstructedGenericType &&
                          t.GetGenericTypeDefinition() == typeof(IHandles<>)
                    select t;

        foreach (var t in query)
        {
            var typeArguments = t.GenericTypeArguments;
            var factory = factoryTemplate?.MakeGenericMethod(typeArguments);
            var handler = (Handler)factory?.Invoke(this, null)!;
            handlers[typeArguments[0]] = handler;
        }

    }

    private Handler GetMessageHandler<TMessage>() where TMessage : class
    {
        // ReSharper disable once SuspiciousTypeConversion.Global
        var handler = (IHandles<TMessage>)this;
        return (envelope, cancellationToken) => handler.Handle(
            new Envelope<TMessage>(envelope.MessageId,
                (TMessage)envelope.Message!,
                envelope.UserId,
                envelope.MessageType), cancellationToken);
    }

    private delegate Task Handler(
        Envelope envelope,
        CancellationToken cancellationToken);
}
