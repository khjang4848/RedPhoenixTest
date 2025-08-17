using Azure.Messaging.ServiceBus;
namespace RedPhoenix.Infrastructure.Azure;

public interface IMessageSender
{
    Task SendAsync(ServiceBusMessage message);
}