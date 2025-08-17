using Azure.Messaging.ServiceBus;

namespace RedPhoenix.Infrastructure.Azure;

using System.Text;
using Messages.DataAnnotations;
using Serialization;

using Messages.Abstraction;

public class MessageBus(IMessageSender messageSender, ITextSerializer textSerializer) : IMessageBus
{
    private readonly IMessageSender _messageSender = messageSender
         ?? throw new ArgumentNullException(nameof(messageSender));
    private readonly ITextSerializer _textSerializer = textSerializer
         ?? throw new ArgumentNullException(nameof(textSerializer));


    public async Task Send(Envelope envelope, CancellationToken cancellationToken)
        => await _messageSender.SendAsync(BuildMessage(envelope));

    public async Task Send(IEnumerable<Envelope> envelopes, CancellationToken cancellationToken)
    {
        foreach (var envelope in envelopes)
        {
            await _messageSender.SendAsync(BuildMessage(envelope));
        }
    }

    private ServiceBusMessage BuildMessage(Envelope envelope)
    {
        var stream = new BinaryData(Encoding.UTF8.GetBytes(
            _textSerializer.Serialize(envelope.Message)));

        var busMessage = new ServiceBusMessage(stream)
        {
            MessageId = envelope.MessageId.ToString("n")
        };

        busMessage.ApplicationProperties.Add("UserId", envelope.UserId);
        busMessage.ApplicationProperties.Add("MessageType", envelope.MessageType);

        if (envelope.UserId != "SYSTEM")
        {
            var lastChar = envelope.UserId[^1];
            busMessage.Subject = (lastChar.GetHashCode() % 4) + "";
        }
        else
        {
            busMessage.Subject = "4";
        }

        return busMessage;
    }
}