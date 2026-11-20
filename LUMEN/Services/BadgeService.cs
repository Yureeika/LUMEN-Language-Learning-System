namespace LUMEN.Services;

using LUMEN.Data;
using LUMEN.Models;
using Microsoft.EntityFrameworkCore;

public class BadgeService(ApplicationDbContext context, INotificationService notificationService) : IBadgeService
{
    public async Task CheckAndAwardBadgesAsync(string userId, CancellationToken cancellationToken = default)
    {
        var user = await context.Users.FindAsync([userId], cancellationToken);
        if (user == null) return;

        var availableBadges = await context.Badges.Where(b => b.IsActive).ToListAsync(cancellationToken);
        var userBadgeIds = await context.UserBadges.Where(ub => ub.UserId == userId).Select(ub => ub.BadgeId).ToListAsync(cancellationToken);

        foreach (var badge in availableBadges)
        {
            if (!userBadgeIds.Contains(badge.Id) && user.TotalXP >= badge.RequiredXP)
            {
                context.UserBadges.Add(new UserBadge { UserId = userId, BadgeId = badge.Id });
                await notificationService.SendNotificationAsync(userId, "New Badge Earned!", $"You earned the {badge.Name} badge!", "/Badges");
            }
        }
        await context.SaveChangesAsync(cancellationToken);
    }
}