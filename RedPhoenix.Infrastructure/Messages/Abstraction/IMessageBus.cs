namespace RedPhoenix.Infrastructure.Messages.Abstraction;

using DataAnnotations;

public interface IMessageBus
{
    Task Send(Envelope envelope, CancellationToken cancellationToken);
    Task Send(IEnumerable<Envelope> envelopes, CancellationToken cancellationToken);
}