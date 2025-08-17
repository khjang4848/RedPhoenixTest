namespace RedPhoenix.Infrastructure.Messages.Abstraction;

using DataAnnotations;

public interface IHandles<TMessage> where TMessage : class
{
    Task Handle(Envelope<TMessage> envelope, CancellationToken cancellationToken);
}