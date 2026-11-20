namespace LUMEN.Services;

using LUMEN.Data;
using LUMEN.Models;

public class AuditService(ApplicationDbContext context) : IAuditService
{
    public async Task LogActionAsync(string userId, string action, string? entityName = null, int? entityId = null, string? oldValues = null, string? newValues = null, string? ipAddress = null, CancellationToken cancellationToken = default)
    {
        var log = new AuditLog { UserId = userId, Action = action, EntityName = entityName, EntityId = entityId, OldValues = oldValues, NewValues = newValues, IpAddress = ipAddress, CreatedAt = DateTime.UtcNow };
        context.AuditLogs.Add(log);
        await context.SaveChangesAsync(cancellationToken);
    }
}

