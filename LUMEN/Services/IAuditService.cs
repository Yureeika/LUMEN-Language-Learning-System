namespace LUMEN.Services;

public interface IAuditService
{
    Task LogActionAsync(string userId, string action, string? entityName = null, int? entityId = null, string? oldValues = null, string? newValues = null, string? ipAddress = null, CancellationToken cancellationToken = default);
}