using Azure.Messaging.ServiceBus;

namespace RedPhoenix.Infrastructure.Azure;
public class TopicSender(string connectionString, string topicName) : IMessageSender
{
    public async Task SendAsync(ServiceBusMessage message)
    {

        await using var client = new ServiceBusClient(connectionString);
        var sender = client.CreateSender(topicName);
        await sender.SendMessageAsync(message);
    }

}