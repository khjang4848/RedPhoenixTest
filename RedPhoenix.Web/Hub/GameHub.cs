using RedPhoenix.Data.Models;
using RedPhoenix.Data.ViewModels;

namespace RedPhoenix.Web.Hub;

using Microsoft.AspNetCore.SignalR;

public class GameHub : Hub

{
    public Task BroadcastMessage(string name, string message) =>
        Clients.All.SendAsync("broadcastMessage", name, message);

    public Task Echo(string name, string message) =>
        Clients.Client(Context.ConnectionId)
            .SendAsync("echo", name, $"{message} (echo from server)");
    

    public async Task UserSendMessage(string user, string message)
        => await Clients.User(user).SendAsync("UserSendMessage", user, message);

    public async Task BettingResult(string user, BettingResultViewModel message)
        => await Clients.User(user).SendAsync("BettingResult", message);

    public async Task BettingFinishResult(string user, BettingFinishResultViewModel message)
    {
        await Clients.User(user).SendAsync("BettingFinishResult", message);
    }

    public async Task BettingWinResult(string group, object message)
    {   
        await Clients.Client(group).SendAsync("BettingWinResult", message);
    }

    public async Task GroupSendMessage(string group, string message)
    {
        await Clients.Client(group).SendAsync("GroupSendMessage", message);
    }

    public async Task JoinGroup(string roomName)
        => await Groups.AddToGroupAsync(Context.ConnectionId, roomName);

    public async Task RemoveGroup(string roomName)
        => await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomName);

}