using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace company.api.Hubs
{
    // Intentionally empty - server pushes messages to clients via IHubContext<NotificationsHub>
    [Authorize]
    public class NotificationsHub : Hub
    {
    }
}
