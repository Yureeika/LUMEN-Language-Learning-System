using Microsoft.AspNetCore.Identity;
namespace LUMEN.Hubs;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

[Authorize]
public class NotificationHub : Hub
{
    public override async Task OnConnectedAsync()
    {
        var userId = Context.UserIdentifier;
        if (!string.IsNullOrEmpty(userId))
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"user_{userId}");
        }
        await base.OnConnectedAsync();
    }

    public async Task SendNotificationToUser(string userId, string title, string message, string? link = null)
    {
        await Clients.Group($"user_{userId}").SendAsync("ReceiveNotification", title, message, link);
    }
}
