using EventConnect.Models.ChatHub;
using EventConnect.Repositories.ChatHubRepository;
using Microsoft.AspNetCore.SignalR;

namespace EventConnect.Services.ChatHub;

public class ChatHubs:Hub
{
    private readonly ConnectionRepository _connectionRepository;

    public ChatHubs(ConnectionRepository connectionRepository) => _connectionRepository = connectionRepository;
    
    
    public async Task JoinChat(UserConnection conn)
    {
        await Clients.All
            .SendAsync("ReceiveMessage", "admin", $"{conn.Username} has joined the chat");
    }
    public async Task JoinSpecificChatRoom(UserConnection conn)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, conn.ChatRoom);
       
        _connectionRepository .connections[Context.ConnectionId]=conn;
        await Clients.Group(conn.ChatRoom)
            .SendAsync("JoinSpecificChatRoom", "admin", $"{conn.Username} has joined the chat");
    }
    public async Task SendMessage(string msg)
    {
        if (_connectionRepository .connections.TryGetValue(Context.ConnectionId, out UserConnection conn))
        {
            await Clients.Group(conn.ChatRoom)
                .SendAsync("ReceiveSpecificMessage", conn.Username, msg);
        }
     
    }
    
}