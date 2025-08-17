namespace RedPhoenix.Infrastructure.Messages.DataAnnotations
{
    public class Envelope<TMessage> : IEnvelope
    {
        public Envelope(Guid messageId, TMessage? message, 
            string? userId = null, string? messageType = null, string? systemType = null)
        {
            if (messageId == Guid.Empty)
            {
                throw new ArgumentException(
                    $"{nameof(messageId)} cannot be empty.",
                    nameof(messageId));
            }

#pragma warning disable IDE0016 // Ignore "Use 'throw' expression" because TMessage does not have a reference type constraint.
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }
#pragma warning restore IDE0016 // Ignore "Use 'throw' expression" because TMessage does not have a reference type constraint.

            MessageId = messageId;
            Message = message;
            UserId = userId;
            MessageType = messageType;
            SystemType = systemType;
        }

        public Guid MessageId { get; }
        public TMessage? Message { get; }
        public string? UserId { get; }
        public string? MessageType { get; }
        public string? SystemType { get; }
        object? IEnvelope.Message => Message;
    }
}