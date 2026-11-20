namespace LUMEN.Hubs;

using LUMEN.Data;
using LUMEN.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

[Authorize]
public class ChatHub(ApplicationDbContext context) : Hub
{
    public async Task SendMessage(int roomId, string message)
    {
        var userId = Context.UserIdentifier;
        var userName = Context.User?.Identity?.Name ?? "Unknown";

        if (string.IsNullOrEmpty(userId)) return;

        var chatMessage = new ChatMessage
        {
            ChatRoomId = roomId,
            UserId = userId,
            Content = message,
            SentAt = DateTime.UtcNow
        };

        context.ChatMessages.Add(chatMessage);
        await context.SaveChangesAsync();

        await Clients.Group(roomId.ToString()).SendAsync("ReceiveMessage", userName, message, chatMessage.SentAt);
    }

    public async Task JoinRoom(int roomId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, roomId.ToString());
        await Clients.Group(roomId.ToString()).SendAsync("UserJoined", Context.UserIdentifier);
    }

    public async Task LeaveRoom(int roomId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomId.ToString());
        await Clients.Group(roomId.ToString()).SendAsync("UserLeft", Context.UserIdentifier);
    }

    public override async Task OnConnectedAsync()
    {
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await base.OnDisconnectedAsync(exception);
    }
}