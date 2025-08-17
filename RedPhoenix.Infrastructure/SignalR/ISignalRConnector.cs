namespace RedPhoenix.Infrastructure.SignalR;
public interface ISignalRConnector
{
    Task SendMessageUser<T>(string method, string user, T message);
    Task SendMessageGroup<T>(string method, string user, T message);
}