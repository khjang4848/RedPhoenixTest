namespace RedPhoenix.Infrastructure.Messages.Abstraction;

using DataAnnotations;

public interface IMessageHandler
{
    bool Accepts(Envelope envelope);
    Task Handle(Envelope envelope, CancellationToken cancellationToken);
}