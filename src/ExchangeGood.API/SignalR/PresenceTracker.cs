namespace ExchangeGood.API.SignalR;

public class PresenceTracker
{
    private static readonly Dictionary<string, List<string>> OnlineUsers = new Dictionary<string, List<string>>(); 

    public Task AddConnection(string feId, string connectionId) {
        lock (OnlineUsers) {
            if(OnlineUsers.ContainsKey(feId)) {
                OnlineUsers[feId].Add(connectionId);
            }
            else {
                OnlineUsers.Add(feId, new List<string> { connectionId });
            }
        }
        return Task.CompletedTask;
    }

    public Task RemoveConnection(string feId, string connectionId) {
        lock(OnlineUsers) {
            if(!OnlineUsers.ContainsKey(feId)) return Task.CompletedTask;

            OnlineUsers[feId].Remove(connectionId);

            if(OnlineUsers[feId].Count == 0) {
                OnlineUsers.Remove(feId);
            }
        }
        return Task.CompletedTask;
    }

    public Task<List<string>> GetConnectionForUser(string feId) {
        List<string> connectionIds = new List<string>();

        lock(OnlineUsers) {
            // get all the values by key -> using method GetValueOrDefault()
            connectionIds = OnlineUsers.GetValueOrDefault(feId);
        }

        return Task.FromResult(connectionIds);
    }
}