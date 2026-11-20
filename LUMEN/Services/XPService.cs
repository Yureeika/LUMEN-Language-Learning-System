namespace LUMEN.Services;

using LUMEN.Data;
using LUMEN.Models;
using Microsoft.EntityFrameworkCore;

public class XPService(ApplicationDbContext context, IBadgeService badgeService) : IXPService
{
    public async Task AddXPAsync(string userId, int amount, string reason, CancellationToken cancellationToken = default)
    {
        var user = await context.Users.FindAsync([userId], cancellationToken);
        if (user == null) return;

        user.TotalXP += amount;
        user.CurrentLevel = ((user.TotalXP / 100) + 1).ToString();

        context.XPLogs.Add(new XPLog { UserId = userId, Amount = amount, Reason = reason });

        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var dailyTrack = await context.DailyTracks.FirstOrDefaultAsync(t => t.UserId == userId && t.TrackDate == today, cancellationToken);

        if (dailyTrack != null) dailyTrack.XPEarned += amount;

        await context.SaveChangesAsync(cancellationToken);
        await badgeService.CheckAndAwardBadgesAsync(userId, cancellationToken);
    }
}


