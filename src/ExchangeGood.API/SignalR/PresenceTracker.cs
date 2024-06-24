namespace ExchangeGood.API.SignalR;

public class PresenceTracker
{
    private static readonly Dictionary<string, List<string>> OnlineUsers = new Dictionary<string, List<string>>(); 

    public Task<bool> UserConnected(string feId, string connectionId) {
        bool isOnline = false;
        lock (OnlineUsers) {
            if(OnlineUsers.ContainsKey(feId)) {
                OnlineUsers[feId].Add(connectionId);
            }
            else {
                OnlineUsers.Add(feId, new List<string> { connectionId });
                isOnline = true;
            }
        }
        return Task.FromResult(isOnline);
    }

    public Task<bool> UserDisconnected(string username, string connectionId) {
        bool isOffline = false;

        lock(OnlineUsers) {
            if(!OnlineUsers.ContainsKey(username)) return Task.FromResult(isOffline);

            OnlineUsers[username].Remove(connectionId);

            if(OnlineUsers[username].Count == 0) {
                OnlineUsers.Remove(username);
                isOffline = true;   
            }
        }

        return Task.FromResult(isOffline);
    }

    public Task<List<string>> GetConnectionForUser(string feId) {
        List<string> connectionIds;

        lock(OnlineUsers) {
            // get all the values by key -> using method GetValueOrDefault()
            connectionIds = OnlineUsers.GetValueOrDefault(feId);
        }

        return Task.FromResult(connectionIds);
    }
}