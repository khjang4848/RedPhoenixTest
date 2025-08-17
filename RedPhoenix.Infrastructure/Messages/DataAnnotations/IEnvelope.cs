namespace RedPhoenix.Infrastructure.Messages.DataAnnotations
{
    public interface IEnvelope
    {
        Guid MessageId { get; }
        object? Message { get; }
       
        string? UserId { get; }
        string? MessageType { get; }
        string? SystemType { get; }
    }
}
