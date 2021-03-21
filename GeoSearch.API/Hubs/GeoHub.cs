using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace GeoSearch.API.Hubs
{
    public class GeoHub : Hub<IGeoHub>
    {
        public Task SendMessage(long currentCount, long currentSum)
        {
            return Clients.Caller.GeoCountUpdate(currentCount, currentSum);
        }
        public string GetConnectionId()
        {
            return Context.ConnectionId;
        }
    }
}
