namespace RedPhoenix.Infrastructure.SignalR;

using Microsoft.AspNetCore.SignalR.Client;

public class SignalRConnector(string hubConnectionString) : ISignalRConnector
{
    private readonly string _hubConnectionString =
        hubConnectionString ??
        throw new ArgumentNullException(nameof(hubConnectionString));

    public async Task SendMessageUser<T>(string method, string user, T message)
    {
        var hubConnection = new HubConnectionBuilder()
            .WithUrl(_hubConnectionString)
            .Build();

        await hubConnection.StartAsync();
        await hubConnection.InvokeAsync(method, user, message);
        await hubConnection.DisposeAsync();
    }

    public async Task SendMessageGroup<T>(string method, string group, T message)
    {
        var hubConnection = new HubConnectionBuilder()
            .WithUrl(_hubConnectionString)
            .Build();

        await hubConnection.StartAsync();
        await hubConnection.InvokeAsync(method, group, message);
        await hubConnection.DisposeAsync();
    }
}
