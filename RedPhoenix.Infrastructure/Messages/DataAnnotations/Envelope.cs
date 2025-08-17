namespace RedPhoenix.Infrastructure.Messages.DataAnnotations
{
    public class Envelope : IEnvelope
    {
        public Envelope(Guid messageId, object? message,
            string? userId = null, string? messageType = null, string? systemType = null)
        {
            if (messageId == Guid.Empty)
            {
                throw new ArgumentException($"{nameof(messageId)} cannot be empty",
                    nameof(messageId));
            }

            MessageId = messageId;
            Message = message ?? throw new ArgumentNullException(nameof(message));
            MessageType = messageType;
            UserId = userId;
            SystemType = systemType;
        }

        public Envelope(object? message,
            string? userId,
            string? messageType, string? systemType)
            : this(messageId: Guid.NewGuid(), message: message, messageType: messageType, 
                userId: userId, systemType: systemType)
        {
        }

        public Guid MessageId { get; }
        public object? Message { get; }
        public string? UserId { get; }
        public string? MessageType { get; }
        public string? SystemType { get; }
    }
}
