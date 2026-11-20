namespace LUMEN.Services;

using LUMEN.Models;

public interface INotificationService
{
    Task SendNotificationAsync(string userId, string title, string message, string? link = null, CancellationToken cancellationToken = default);
    Task<IEnumerable<Notification>> GetUnreadNotificationsAsync(string userId, CancellationToken cancellationToken = default);
}