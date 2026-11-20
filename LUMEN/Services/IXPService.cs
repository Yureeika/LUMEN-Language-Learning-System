namespace LUMEN.Services;

public interface IXPService
{
    Task AddXPAsync(string userId, int amount, string reason, CancellationToken cancellationToken = default);
}