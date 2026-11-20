namespace LUMEN.Services;

using LUMEN.Data;
using LUMEN.Hubs;
using LUMEN.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

public class NotificationService(ApplicationDbContext context, IHubContext<NotificationHub> hubContext) : INotificationService
{
    public async Task SendNotificationAsync(string userId, string title, string message, string? link = null, CancellationToken cancellationToken = default)
    {
        var notification = new Notification { UserId = userId, Title = title, Message = message, Link = link, CreatedAt = DateTime.UtcNow };
        context.Notifications.Add(notification);
        await context.SaveChangesAsync(cancellationToken);
        await hubContext.Clients.Group($"user_{userId}").SendAsync("ReceiveNotification", title, message, link, cancellationToken);
    }

    public async Task<IEnumerable<Notification>> GetUnreadNotificationsAsync(string userId, CancellationToken cancellationToken = default)
    {
        return await context.Notifications.Where(n => n.UserId == userId && !n.IsRead).OrderByDescending(n => n.CreatedAt).ToListAsync(cancellationToken);
    }
}