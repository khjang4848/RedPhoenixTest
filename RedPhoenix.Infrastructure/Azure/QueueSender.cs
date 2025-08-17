using Azure.Messaging.ServiceBus;

namespace RedPhoenix.Infrastructure.Azure;

public class QueueSender(string connectionString, string queueName) : IMessageSender
{
    public async Task SendAsync(ServiceBusMessage message)
    {
        await using var client = new ServiceBusClient(connectionString);
        var sender = client.CreateSender(queueName);
        await sender.SendMessageAsync(message);
    }
}
