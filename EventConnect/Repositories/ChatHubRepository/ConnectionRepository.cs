using System.Collections.Concurrent;
using EventConnect.Models.ChatHub;

namespace EventConnect.Repositories.ChatHubRepository;

public class ConnectionRepository
{
    private readonly ConcurrentDictionary<string, UserConnection> _connections = new (); 
    public ConcurrentDictionary<string, UserConnection> connections => _connections;
}