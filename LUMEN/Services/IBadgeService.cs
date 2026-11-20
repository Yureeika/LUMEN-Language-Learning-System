namespace LUMEN.Services;

public interface IBadgeService
{
    Task CheckAndAwardBadgesAsync(string userId, CancellationToken cancellationToken = default);
}